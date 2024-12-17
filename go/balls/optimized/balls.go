package main

import (
	"fmt"
	"sort"
)

func main() {
	var L, n, q int
	fmt.Scanf("%d %d %d\n", &L, &n, &q)
	// convert to zero index
	L--

	positions := make([]int, n)
	for i := 0; i < n; i++ {
		fmt.Scan(&positions[i])
		// convert to zero index
		positions[i]--
	}

	for i := 0; i < n; i++ {
		var direction int
		fmt.Scan(&direction)
		if direction == 0 {
			positions[i] = L - positions[i]
		} else {
			positions[i] += L
		}
	}

	sort.Slice(positions, func(i, j int) bool {
		return positions[i] < positions[j]
	})

	for i := 0; i < q; i++ {
		var time, pos int
		fmt.Scanf("%d %d\n", &time, &pos)

		// we need to use half second time intervals
		time = (L - time) % (2 * L)
		if time < 0 {
			time += 2 * L
		}

		bottom, top := 0, L
		var result int
		for bottom <= top {
			middle := (bottom + top) / 2
			up := (time + middle) % (2 * L)
			low := (time - middle) % (2 * L)
			if low < 0 {
				low += 2 * L
			}

			var center int
			if middle == L {
				center = n
			} else if up >= low {
				center = upperBound(positions, up) - lowerBound(positions, low)
			} else {
				center = n - (lowerBound(positions, low) - upperBound(positions, up))
			}

			if center < pos {
				bottom = middle + 1
			} else {
				top = middle - 1
				result = middle
			}
		}

		// increment position due to the zero index
		fmt.Println(result + 1)
	}
}

// lowerBound finds the first index where the value is not less than `val`.
// binary search is used in sorted array
func lowerBound(arr []int, val int) int {
	left, right := 0, len(arr)
	for left < right {
		middle := (left + right) / 2
		if arr[middle] < val {
			left = middle + 1
		} else {
			right = middle
		}
	}
	return left
}

// upperBound finds the first index where the value is greater than `val`.
// binary search is used in sorted array
func upperBound(arr []int, val int) int {
	left, right := 0, len(arr)
	for left < right {
		middle := (left + right) / 2
		if arr[middle] <= val {
			left = middle + 1
		} else {
			right = middle
		}
	}
	return left
}
