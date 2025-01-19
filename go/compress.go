package main

import (
	"fmt"
	"strconv"
)

func compress(input string) string {
	if len(input) == 0 {
		return ""
	}

	var result []byte
	count := 1

	for i := 1; i <= len(input); i++ {
		// If current character is same as the previous one, increment the count
		if i < len(input) && input[i] == input[i-1] {
			count++
		} else {
			// Append the character
			result = append(result, input[i-1])

			// Append the count only if it's more than 2
			if count > 2 {
				result = append(result, []byte(strconv.Itoa(count))...)
			} else if count == 2 {
				// If the character appears exactly twice, append one more of the same character
				result = append(result, input[i-1])
			}
			// Reset count for the next character
			count = 1
		}
	}

	return string(result)
}

func main() {
  test("basic example", "aaaabbCddd", "a4bbCd3")
  test("empty test", "", "")
  test("no compression", "abcd", "abcd")
  test("max compression", "aaaaaaa", "a7")
}

func test(name string, input string, expected string) {
  fmt.Println("[test] " + name)

  result := compress(input)

  if result == expected {
    fmt.Println("  V  passed")
  } else {
    fmt.Println("  X  failed")
    fmt.Println("  Result:   " + result)
    fmt.Println("  Expected: " + expected)
  }
  
  fmt.Println()
}
