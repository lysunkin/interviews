package main

import (
	"fmt"
	"math/rand"
	"sync"
	"time"
)

// Buffer size
const bufferSize = 5

func main() {
	// Shared channel acting as the buffer
	buffer := make(chan int, bufferSize)

	// WaitGroup to synchronize the main function with goroutines
	var wg sync.WaitGroup

	// Number of producers and consumers
	numProducers := 3
	numConsumers := 2

	// Producer function
	producer := func(id int) {
		defer wg.Done()
		for i := 0; i < 10; i++ {
			item := rand.Intn(100) // Produce a random item
			buffer <- item         // Add item to the buffer
			fmt.Printf("Producer %d produced: %d\n", id, item)
			time.Sleep(time.Millisecond * time.Duration(rand.Intn(500)))
		}
	}

	// Consumer function
	consumer := func(id int) {
		defer wg.Done()
		for {
			select {
			case item := <-buffer: // Consume item from the buffer
				fmt.Printf("Consumer %d consumed: %d\n", id, item)
				time.Sleep(time.Millisecond * time.Duration(rand.Intn(500)))
			case <-time.After(time.Second): // Exit if no items are produced for 1 second
				fmt.Printf("Consumer %d is exiting due to inactivity\n", id)
				return
			}
		}
	}

	// Start producers
	for i := 0; i < numProducers; i++ {
		wg.Add(1)
		go producer(i + 1)
	}

	// Start consumers
	for i := 0; i < numConsumers; i++ {
		wg.Add(1)
		go consumer(i + 1)
	}

	// Wait for all producers and consumers to finish
	wg.Wait()
	fmt.Println("All producers and consumers have finished.")
}
