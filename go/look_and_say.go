package main

import (
	"fmt"
	"strings"
)

// printLookAndSay prints the first n sequences of the Look-and-say sequence.
func printLookAndSay(n int) {
	current := "1"
	for i := 1; i <= n; i++ {
		fmt.Println(current)
		current = getNextLookAndSay(current)
	}
}

// getNextLookAndSay generates the next sequence in the Look-and-say sequence.
func getNextLookAndSay(val string) string {
	var result strings.Builder
	count := 1
	current := val[0]

	for i := 1; i < len(val); i++ {
		if val[i] == current {
			count++
		} else {
			result.WriteString(fmt.Sprintf("%d%c", count, current))
			current = val[i]
			count = 1
		}
	}
	result.WriteString(fmt.Sprintf("%d%c", count, current))

	return result.String()
}

func main() {
	printLookAndSay(10)
}
