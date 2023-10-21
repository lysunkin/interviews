package main

import (
	"fmt"
)

func islands(islandMap [][]int) int {

	rows := len(islandMap)
	fmt.Println("rows =", rows)

	var cols int
	if rows > 0 {
		cols = len(islandMap[0])
		fmt.Println("cols =", cols)
	}

	if rows == 0 || cols == 0 {
		return 0
	}

	numIslands := 0
	visited := make([][]bool, rows)

	for i := 0; i < rows; i++ {
		visited[i] = make([]bool, cols)
	}

	for i := 0; i < rows; i++ {
		for j := 0; j < cols; j++ {
			if islandMap[i][j] == 1 && !visited[i][j] {
				numIslands++
				move(islandMap, visited, i, j)
			}
		}
	}

	return numIslands
}

func move(islandMap [][]int, visited [][]bool, i, j int) {
	if i < 0 || i >= len(islandMap) {
		return
	}

	if j < 0 || j >= len(islandMap[i]) {
		return
	}

	if islandMap[i][j] == 1 && !visited[i][j] {
		visited[i][j] = true
		move(islandMap, visited, i-1, j)
		move(islandMap, visited, i, j-1)
		move(islandMap, visited, i+1, j)
		move(islandMap, visited, i, j+1)
	}
}

func main() {
	islands1 := [][]int{
		{1, 1, 0, 1},
		{1, 1, 0, 0},
		{0, 0, 0, 0},
	}

	islands2 := [][]int{
		{1, 1, 0, 1, 0, 0},
		{1, 1, 0, 0, 0, 1},
		{0, 0, 0, 0, 0, 1},
	}

	islands3 := [][]int{
		{1, 1, 0, 1, 0, 0},
		{1, 1, 0, 0, 0, 1},
		{0, 0, 0, 1, 0, 1},
		{0, 1, 0, 1, 0, 1},
		{0, 1, 1, 1, 0, 1},
	}

	fmt.Println(islands(islands1))
	fmt.Println(islands(islands2))
	fmt.Println(islands(islands3))
}
