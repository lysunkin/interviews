package main

import (
	"fmt"
	"math/rand"
	"sort"
)

func main() {
	length := 10
	array1 := make([]int, length)
	array2 := make([]int, length*2+1)

	for i := 0; i < len(array1); i++ {
		array1[i] = rand.Intn(100) + 1
	}

	for i := 0; i < len(array2); i++ {
		array2[i] = rand.Intn(100) + 1
	}

	sort.Ints(array1)
	sort.Ints(array2)

	fmt.Println("First: ", array1)
	fmt.Println("Second:", array2)

	// Naive
	arrayN := append(array1, array2...)
	sort.Ints(arrayN)
	fmt.Println("Naive: ", arrayN)

	array := make([]int, len(array1)+len(array2))
	pos := 0
	pos1 := 0
	pos2 := 0

	for pos1 < len(array1) && pos2 < len(array2) {
		if array1[pos1] < array2[pos2] {
			array[pos] = array1[pos1]
			pos1++
		} else {
			array[pos] = array2[pos2]
			pos2++
		}
		pos++
	}

	for pos1 < len(array1) {
		array[pos] = array1[pos1]
		pos1++
		pos++
	}

	for pos2 < len(array2) {
		array[pos] = array2[pos2]
		pos2++
		pos++
	}

	fmt.Println("Merged:", array)
}
