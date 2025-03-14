/*
Find the maximum length of a contiguous substring within a given string that consists of no more than k unique characters.

abbacaabaa, k=2 -> len(aabaa) == 5
abbacccccc, k=2 -> len(acccccc) == 7
*/

package main

import "fmt"

func maxContLen(s string, k int) int {
	if k <= 0 || len(s) == 0 {
		return 0
	}

	left, maxLen := 0, 0
	d := make(map[byte]int)

	for right := 0; right < len(s); right++ {
		d[s[right]]++

		for len(d) > k {
			d[s[left]]--
			if d[s[left]] == 0 {
				delete(d, s[left])
			}
			left++
		}

		if right-left+1 > maxLen {
			maxLen = right - left + 1
		}
	}

	return maxLen
}

func main() {
	fmt.Println(maxContLen("abbacaabaa", 2))  // => 5
	fmt.Println(maxContLen("abbacccccc", 2))  // => 7
	fmt.Println(maxContLen("abbacaabaa", 26)) // => 10
	fmt.Println(maxContLen("abcdefg", 2))     // => 2
	fmt.Println(maxContLen("abcdefg", -1))    // => 0
	fmt.Println(maxContLen("", 10))           // => 0
	fmt.Println(maxContLen("a", 10))          // => 1
}
