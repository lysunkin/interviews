package main

import (
	"fmt"
	"io"
	"net/http"
	"regexp"
	"sync"
)

func main() {
	// List of URLs to scrape
	urls := []string{
		"https://golang.org",
		"https://www.google.com",
		"https://www.github.com",
		"https://nonexistent.url",
	}

	// Channel to collect results
	results := make(chan string)
	var wg sync.WaitGroup

	// Semaphore to limit concurrency (e.g., max 3 workers)
	concurrencyLimit := 3
	semaphore := make(chan struct{}, concurrencyLimit)

	// Start scraping URLs
	for _, url := range urls {
		wg.Add(1)

		// Use a goroutine for each URL
		go func(url string) {
			defer wg.Done()
			semaphore <- struct{}{}        // Acquire a semaphore slot
			defer func() { <-semaphore }() // Release the semaphore slot

			title, err := fetchTitle(url)
			if err != nil {
				results <- fmt.Sprintf("Error fetching %s: %v", url, err)
				return
			}
			results <- fmt.Sprintf("Title of %s: %s", url, title)
		}(url)
	}

	// Close the results channel after all goroutines are done
	go func() {
		wg.Wait()
		close(results)
	}()

	// Print the results
	for result := range results {
		fmt.Println(result)
	}
}

// fetchTitle fetches the title of a webpage from a given URL
func fetchTitle(url string) (string, error) {
	resp, err := http.Get(url)
	if err != nil {
		return "", err
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return "", fmt.Errorf("HTTP status %d", resp.StatusCode)
	}

	body, err := io.ReadAll(resp.Body)
	if err != nil {
		return "", err
	}

	// Extract the title using a regex
	titleRegex := regexp.MustCompile(`<title>(.*?)</title>`)
	matches := titleRegex.FindStringSubmatch(string(body))
	if len(matches) < 2 {
		return "No title found", nil
	}

	return matches[1], nil
}
