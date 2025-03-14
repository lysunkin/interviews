package main

import (
	"fmt"
	"math/rand"
	"sync"
	"time"
)

// simulateAPIRequest simulates processing an API request and returns a result.
func simulateAPIRequest(requestID int) string {
	// Simulate a random processing time
	time.Sleep(time.Duration(rand.Intn(500)) * time.Millisecond)
	return fmt.Sprintf("Response from Request %d", requestID)
}

// rateLimitedAPI processes requests with rate limiting.
func rateLimitedAPI(requests []int, rateLimit int) []string {
	results := make([]string, len(requests))
	var wg sync.WaitGroup
	ticker := time.NewTicker(time.Second / time.Duration(rateLimit))
	defer ticker.Stop()

	// Channel to pass the request IDs
	requestChannel := make(chan int)

	// Worker goroutine
	go func() {
		for requestID := range requestChannel {
			<-ticker.C // Rate limit: wait for the next tick
			wg.Add(1)
			go func(reqID int) {
				defer wg.Done()
				results[reqID] = simulateAPIRequest(reqID)
			}(requestID)
		}
	}()

	// Send requests
	for i := range requests {
		requestChannel <- i
	}
	close(requestChannel)

	// Wait for all workers to complete
	wg.Wait()
	return results
}

func main() {
	// Example requests
	requests := make([]int, 10)
	for i := range requests {
		requests[i] = i
	}

	// Rate limit (e.g., 5 requests per second)
	rateLimit := 5

	// Process requests
	fmt.Println("Processing requests with rate limiting...")
	start := time.Now()
	results := rateLimitedAPI(requests, rateLimit)
	elapsed := time.Since(start)

	// Print results
	for _, result := range results {
		fmt.Println(result)
	}

	fmt.Printf("Processed %d requests in %v\n", len(requests), elapsed)
}
