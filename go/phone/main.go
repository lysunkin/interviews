package main

import (
	"fmt"
	"main/api"
	"net/http"
)

func main() {
	http.HandleFunc("/v1/phone-numbers", api.PhoneNumbersHandler)
	fmt.Println("Server started at :8080")
	http.ListenAndServe(":8080", nil)
}
