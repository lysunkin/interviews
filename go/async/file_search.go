package main

import (
	"bufio"
	"fmt"
	"os"
	"path/filepath"
	"strings"
	"sync"
)

// searchFile searches for a keyword in a file and sends matches to the results channel.
func searchFile(filePath, keyword string, results chan<- string) {
	file, err := os.Open(filePath)
	if err != nil {
		results <- fmt.Sprintf("Error opening file %s: %v", filePath, err)
		return
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	lineNumber := 0
	buf := make([]byte, 0, 64*1024)
	scanner.Buffer(buf, 1024*1024)
	for scanner.Scan() {
		lineNumber++
		if strings.Contains(scanner.Text(), keyword) {
			results <- fmt.Sprintf("Match found in %s [Line %d]: %s", filePath, lineNumber, scanner.Text())
		}
	}

	if err := scanner.Err(); err != nil {
		results <- fmt.Sprintf("Error reading file %s: %v", filePath, err)
	}
}

// traverseDirectory traverses a directory recursively and sends file paths to the file channel.
func traverseDirectory(root string, fileChan chan<- string, wg *sync.WaitGroup) {
	defer wg.Done()

	err := filepath.Walk(root, func(path string, info os.FileInfo, err error) error {
		if err != nil {
			return err
		}
		if !info.IsDir() {
			fileChan <- path
		}
		return nil
	})

	if err != nil {
		fmt.Printf("Error traversing directory %s: %v\n", root, err)
	}
}

func main() {
	// Input parameters
	rootDir := "/home/slysunkin/Downloads"
	keyword := "PDF-1."

	// Channels for communication
	fileChan := make(chan string)
	results := make(chan string)

	var wg sync.WaitGroup

	// Traverse directories
	wg.Add(1)
	go traverseDirectory(rootDir, fileChan, &wg)

	// Process files concurrently
	go func() {
		wg.Wait()
		close(fileChan)
	}()

	var fileSearchWG sync.WaitGroup

	// Worker goroutines to search files
	go func() {
		for filePath := range fileChan {
			fileSearchWG.Add(1)
			go func(filePath string) {
				defer fileSearchWG.Done()
				searchFile(filePath, keyword, results)
			}(filePath)
		}

		// Close results channel when all file searches are done
		fileSearchWG.Wait()
		close(results)
	}()

	// Collect and print results
	for result := range results {
		fmt.Println(result)
	}
}
