package main

import "fmt"

// snail generates a snail matrix of size n x n
// The snail matrix is filled in a spiral order starting from the top-left corner
// and moving right, down, left, and up in a clockwise direction.
func snail(n int) [][]int {
	result := make([][]int, n)
	for i := 0; i < n; i++ {
		result[i] = make([]int, n)
	}

	x, y := 0, 0
	directions := [][]int{{0, 1}, {1, 0}, {0, -1}, {-1, 0}}
	direction := 0
	for i := 1; i <= n*n; i++ {
		result[x][y] = i
		// try to move
		newX, newY := x+directions[direction][0], y+directions[direction][1]
		if !canMove(result, newX, newY) {
			// change direction
			direction = (direction + 1) % 4
			newX, newY = x+directions[direction][0], y+directions[direction][1]
		}
		x, y = newX, newY
	}

	return result
}

func canMove(matrix [][]int, x, y int) bool {
	return x >= 0 && x < len(matrix) && y >= 0 && y < len(matrix) && matrix[x][y] == 0
}

func main() {
	data := snail(9)
	for i := 0; i < len(data); i++ {
		for j := 0; j < len(data); j++ {
			fmt.Printf("%02d ", data[i][j])
		}
		fmt.Println()
	}
}
