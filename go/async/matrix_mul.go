package main

import (
	"fmt"
	"sync"
)

// multiplyMatrices performs parallel matrix multiplication.
func multiplyMatrices(matrixA, matrixB [][]int) ([][]int, error) {
	// Validate matrices dimensions
	rowsA := len(matrixA)
	colsA := len(matrixA[0])
	rowsB := len(matrixB)
	colsB := len(matrixB[0])

	if colsA != rowsB {
		return nil, fmt.Errorf("matrices dimensions mismatch: cannot multiply %dx%d and %dx%d", rowsA, colsA, rowsB, colsB)
	}

	// Initialize the result matrix
	result := make([][]int, rowsA)
	for i := range result {
		result[i] = make([]int, colsB)
	}

	// Use a WaitGroup to wait for all goroutines to finish
	var wg sync.WaitGroup

	// Perform the multiplication in parallel
	for i := 0; i < rowsA; i++ {
		for j := 0; j < colsB; j++ {
			wg.Add(1)
			go func(i, j int) {
				defer wg.Done()
				for k := 0; k < colsA; k++ {
					result[i][j] += matrixA[i][k] * matrixB[k][j]
				}
			}(i, j)
		}
	}

	// Wait for all computations to finish
	wg.Wait()
	return result, nil
}

// printMatrix prints a matrix in a readable format.
func printMatrix(matrix [][]int) {
	for _, row := range matrix {
		fmt.Println(row)
	}
}

func main() {
	// Example matrices
	matrixA := [][]int{
		{1, 2, 3},
		{4, 5, 6},
	}

	matrixB := [][]int{
		{7, 8},
		{9, 10},
		{11, 12},
	}

	// Multiply matrices
	result, err := multiplyMatrices(matrixA, matrixB)
	if err != nil {
		fmt.Println("Error:", err)
		return
	}

	// Print the result
	fmt.Println("Result of Matrix Multiplication:")
	printMatrix(result)
}
