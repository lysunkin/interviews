package main

import (
	"fmt"
	"time"
)

// fibonacci calculates the Fibonacci number for a given `n` recursively.
func fibonacci(n int, resultChan chan<- string) {
	if n <= 1 {
		resultChan <- fmt.Sprintf("Fibonacci(%d) = %d", n, n)
		return
	}
	resultChan <- fmt.Sprintf("Fibonacci(%d) = %d", n, fibonacciRecursive(n))
}

// fibonacciRecursive is a helper function to calculate Fibonacci recursively.
func fibonacciRecursive(n int) int {
	if n <= 1 {
		return n
	}
	return fibonacciRecursive(n-1) + fibonacciRecursive(n-2)
}

func measureExecutionTime(start time.Time) {
	elapsed := time.Since(start)
	fmt.Printf("Execution time: %s\n", elapsed)
}

func main() {
	// Range of inputs to calculate Fibonacci numbers for
	inputs := []int{5, 7, 10, 12, 15, 20, 25, 30, 35}

	defer measureExecutionTime(time.Now())

	// Channel for results from goroutines
	resultChan := make(chan string, len(inputs))

	// Launch separate goroutines to calculate Fibonacci numbers
	for _, input := range inputs {
		go fibonacci(input, resultChan)
	}

	// Collect results from goroutines
	for i := 0; i < len(inputs); i++ {
		result := <-resultChan
		fmt.Println(result)
	}
}
