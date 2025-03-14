package main

import (
	"fmt"
	"sync"
	"time"
)

// fibonacci calculates the Fibonacci number for a given `n`.
func fibonacci(n int) int {
	if n <= 1 {
		return n
	}
	return fibonacci(n-1) + fibonacci(n-2)
}

// worker calculates Fibonacci numbers from the task channel and sends the results to the results channel.
func worker(taskChan <-chan int, resultChan chan<- string, wg *sync.WaitGroup) {
	defer wg.Done()
	for n := range taskChan {
		result := fibonacci(n)
		resultChan <- fmt.Sprintf("Fibonacci(%d) = %d", n, result)
	}
}

func measureExecutionTime(start time.Time) {
	elapsed := time.Since(start)
	fmt.Printf("Execution time: %s\n", elapsed)
}

func sequentialFibonacciTest(inputs []int) {
	defer measureExecutionTime(time.Now())

	for _, input := range inputs {
		fmt.Printf("Fibonacci(%d) = %d\n", input, fibonacci(input))
	}
}

func asyncFibonacciTest(inputs []int) {
	defer measureExecutionTime(time.Now())

	// Channels for tasks and results
	taskChan := make(chan int, len(inputs))
	resultChan := make(chan string, len(inputs))

	// WaitGroup to synchronize worker goroutines
	var wg sync.WaitGroup

	// Number of concurrent workers
	numWorkers := 9

	// Start worker goroutines
	for i := 0; i < numWorkers; i++ {
		wg.Add(1)
		go worker(taskChan, resultChan, &wg)
	}

	// Send tasks to the task channel
	for _, input := range inputs {
		taskChan <- input
	}
	close(taskChan) // Close the task channel to signal workers that no more tasks will be sent

	// Wait for all workers to finish
	go func() {
		wg.Wait()
		close(resultChan) // Close the results channel once all workers are done
	}()

	// Collect and print results
	for result := range resultChan {
		fmt.Println(result)
	}
}

func main() {
	// Range of inputs to calculate Fibonacci numbers for
	inputs := []int{5, 7, 10, 12, 15, 20, 25, 30, 35}

	sequentialFibonacciTest(inputs)
	asyncFibonacciTest(inputs)
	fmt.Println("All tests completed.")
}
