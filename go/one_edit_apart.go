package main

import (
	"fmt"
)

// oneEditApart checks if two strings are one edit apart
func oneEditApart(s1, s2 string) bool {
	// Ensure s1 is the shorter string
	if len(s1) > len(s2) {
		s1, s2 = s2, s1
	}

	// If the length difference is more than 1, they can't be one edit apart
	if len(s2)-len(s1) > 1 {
		return false
	}

	edited := false
	j := 0

	for i := 0; i < len(s1); i++ {
		if s1[i] != s2[j] {
			if edited {
				return false
			}
			edited = true
			if len(s2) > len(s1) {
				i-- // Decrement i to stay on the same character in s1
			}
		}
		j++
	}

	// If strings are of different lengths, the last character of s2 is the edit
	return edited || len(s1) != len(s2)
}

func main() {
	fmt.Println(oneEditApart("cat", "dog"))    // false
	fmt.Println(oneEditApart("cat", "cats"))   // true
	fmt.Println(oneEditApart("cat", "cut"))    // true
	fmt.Println(oneEditApart("cat", "cast"))   // true
	fmt.Println(oneEditApart("cat", "at"))     // true
	fmt.Println(oneEditApart("cat", "act"))    // false
	fmt.Println(oneEditApart("cat", "cattle")) // false
}
