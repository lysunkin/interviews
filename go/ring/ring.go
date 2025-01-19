package main

import (
	"errors"
	"fmt"
	"sync"
)

type RingBuffer[T any] struct {
	data  []T
	size  int
	lock  sync.RWMutex
	start int
	end   int
	count int
}

// New creates a new RingBuffer with the given size.
func New[T any](size int) *RingBuffer[T] {
	return &RingBuffer[T]{
		data: make([]T, size),
		size: size,
	}
}

// Push adds an element to the ring buffer.
func (rb *RingBuffer[T]) Push(elem T) {
	rb.lock.Lock()
	defer rb.lock.Unlock()

	rb.data[rb.start] = elem
	rb.start = (rb.start + 1) % rb.size

	if rb.count < rb.size {
		rb.count++
	} else {
		rb.end = (rb.end + 1) % rb.size
	}
}

// Pull removes and returns an element from the ring buffer.
func (rb *RingBuffer[T]) Pull() (T, error) {
	rb.lock.Lock()
	defer rb.lock.Unlock()

	if rb.count == 0 {
		return *new(T), errors.New("empty buffer")
	}

	elem := rb.data[rb.end]
	rb.end = (rb.end + 1) % rb.size
	rb.count--

	return elem, nil
}

// MaxSize returns the maximum size of the ring buffer.
func (rb *RingBuffer[T]) MaxSize() int {
	return rb.size
}

// Length returns the current number of elements in the ring buffer.
func (rb *RingBuffer[T]) Length() int {
	rb.lock.RLock()
	defer rb.lock.RUnlock()

	return rb.count
}

// Empty clears the ring buffer.
func (rb *RingBuffer[T]) Empty() {
	rb.lock.Lock()
	defer rb.lock.Unlock()

	rb.start = 0
	rb.end = 0
	rb.count = 0
	rb.data = make([]T, rb.size)
}

// Snapshot returns a snapshot of the current elements in the ring buffer.
func (rb *RingBuffer[T]) Snapshot() []T {
	rb.lock.RLock()
	defer rb.lock.RUnlock()

	result := make([]T, rb.count)
	for i := 0; i < rb.count; i++ {
		result[i] = rb.data[(rb.end+i)%rb.size]
	}

	return result
}

func main() {
	const size = 8

	buff := New[int](size)
	fmt.Println("MaxSize:", buff.MaxSize())
	fmt.Println("Length:", buff.Length())
	fmt.Println("Content:", buff.Snapshot())

	for i := 1; i <= size; i++ {
		buff.Push(i)
	}
	fmt.Println("Length:", buff.Length())
	fmt.Println("Content:", buff.Snapshot())

	buff.Push(9)
	buff.Push(0)
	fmt.Println("Length:", buff.Length())
	fmt.Println("Content:", buff.Snapshot())

	for i := 0; i < size/2; i++ {
		elem, _ := buff.Pull()
		fmt.Println(elem)
	}

	fmt.Println("Length:", buff.Length())
	fmt.Println("Content:", buff.Snapshot())

	for i := 0; i < size/2; i++ {
		elem, _ := buff.Pull()
		fmt.Println(elem)
	}

	fmt.Println("Length:", buff.Length())
	fmt.Println("Content:", buff.Snapshot())

	elem, err := buff.Pull()
	if err != nil {
		fmt.Println(err)
	} else {
		fmt.Println(elem)
	}

	for i := 1; i <= size; i++ {
		buff.Push(i)
	}
	fmt.Println("Length:", buff.Length())
	fmt.Println("Content:", buff.Snapshot())

	buff.Empty()
	fmt.Println("Length:", buff.Length())
	fmt.Println("Content:", buff.Snapshot())
}
