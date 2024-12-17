package main

import (
	"fmt"
)

func main() {
	var L, n, q int
	// Read the first line: L, n, q
	fmt.Scanf("%d %d %d", &L, &n, &q)

	// Read the second line: n distinct integers (positions of balls)
	balls := make([]int, n)
	for i := 0; i < n; i++ {
		fmt.Scan(&balls[i])
	}

	// Read the third line: n integers (directions of balls, 0 for left, 1 for right)
	directions := make([]int, n)
	for i := 0; i < n; i++ {
		fmt.Scan(&directions[i])
	}

	// convert initial positions to absolute positions keeping in mind the fact that we need to move in half steps
	positionAbs := make([]int, n)
	for i := 0; i < n; i++ {
		positionAbs[i] = (balls[i] - 1) * 2
	}

	// Process each query
	for i := 0; i < q; i++ {
		var ti, pi int
		// Read query: time ti and ball index pi (1-based)
		fmt.Scan(&ti, &pi)
		pi-- // Convert to 0-based index

		// Create arrays to track the direction and position of the balls
		direction := make([]int, n)
		copy(direction, directions)
		position := make([]int, n)
		copy(position, positionAbs)

		// Simulate the movements in half-second steps
		for t := 0; t < 2*ti; t++ { // Each second is split into two half-seconds
			for j := 0; j < n; j++ {
				if direction[j] == 1 {
					position[j]++ // Move right
				} else {
					position[j]-- // Move left
				}

				// Handle collisions with the walls
				if position[j] == -1 {
					direction[j] = 1 // Bounce to the right
					position[j] = 1
				} else if position[j] == L*2 {
					direction[j] = 0 // Bounce to the left
					position[j] = L*2 - 1
				}
			}

			// Handle collisions between adjacent balls (neighbors only)
			for j := 0; j < n-1; j++ {
				// Check if two adjacent balls are moving toward each other
				if position[j] == position[j+1] && direction[j] == 1 && direction[j+1] == 0 {
					// Reverse their directions
					direction[j], direction[j+1] = direction[j+1], direction[j]
				}
			}
		}

		// Output the final position of the pi-th ball after ti seconds
		fmt.Println(position[pi]/2 + 1)
	}
}
