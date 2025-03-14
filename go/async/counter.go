package main

import (
	"fmt"
	"sync"
	"time"
)

// DistributedCounter simulates a distributed counter with synchronization.
type DistributedCounter struct {
	mu      sync.RWMutex
	counter int
}

// Increment increments the counter by the given value.
func (dc *DistributedCounter) Increment(value int) {
	dc.mu.Lock()
	defer dc.mu.Unlock()
	dc.counter += value
	fmt.Printf("Counter incremented by %d, new value: %d\n", value, dc.counter)
}

// GetValue retrieves the current value of the counter.
func (dc *DistributedCounter) GetValue() int {
	dc.mu.RLock()
	defer dc.mu.RUnlock()
	return dc.counter
}

func main() {
	// Create a distributed counter
	counter := &DistributedCounter{}

	var wg sync.WaitGroup
	numGoroutines := 10

	// Simulate multiple goroutines incrementing the counter
	for i := 0; i < numGoroutines; i++ {
		wg.Add(1)
		go func(id int) {
			defer wg.Done()
			counter.Increment(1)
			time.Sleep(100 * time.Millisecond) // Simulate work
			fmt.Printf("Goroutine %d reads counter value: %d\n", id, counter.GetValue())
		}(i)
	}

	// Wait for all goroutines to finish
	wg.Wait()

	// Final value of the counter
	fmt.Printf("Final counter value: %d\n", counter.GetValue())
}
