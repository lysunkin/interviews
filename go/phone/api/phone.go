package api

import (
	"encoding/json"
	"net/http"
	"regexp"
	"strings"

	"github.com/dongri/phonenumber"
)

const AREA_CODE_LEN = 3

type PhoneNumberResponse struct {
	PhoneNumber      string `json:"phoneNumber"`
	CountryCode      string `json:"countryCode,omitempty"`
	AreaCode         string `json:"areaCode,omitempty"`
	LocalPhoneNumber string `json:"localPhoneNumber,omitempty"`
	Error            string `json:"error,omitempty"`
}

// ValidatePhoneNumber validates phone number and returns parsed results and validation result.
func ValidatePhoneNumber(phoneNumber, countryCode string) *PhoneNumberResponse {
	// verify phone format according to the task description (i.e. no extra spaces; only spaces, digits and optional +)
	match, err := regexp.MatchString(`^\+?(\d{1,3})\ ?(\d{1,4})\ ?(\d{6,10})$`, phoneNumber)
	if !match || err != nil {
		return &PhoneNumberResponse{Error: "invalid phone number format"}
	}

	// check if phone can be found
	number := phonenumber.ParseWithLandLine(phoneNumber, countryCode)
	if number == "" {
		return &PhoneNumberResponse{Error: "country code is required"}
	}

	// retrieve the country information
	country := phonenumber.GetISO3166ByNumber(number, true)
	if countryCode != "" && countryCode != country.Alpha2 {
		return &PhoneNumberResponse{Error: "country code is invalid"}
	}

	// parse area code and local number using country code and assuming area has 3 characters
	// (actually I don't know how long the area code can be)
	internalNumber := strings.TrimPrefix(number, country.CountryCode)
	area := internalNumber[:AREA_CODE_LEN]
	local := internalNumber[AREA_CODE_LEN:]

	return &PhoneNumberResponse{
		PhoneNumber:      "+" + number,
		CountryCode:      country.Alpha2,
		AreaCode:         area,
		LocalPhoneNumber: local,
		Error:            "",
	}
}

// PhoneNumbersHandler provides API entry point.
func PhoneNumbersHandler(w http.ResponseWriter, r *http.Request) {
	phoneNumber := r.URL.Query().Get("phoneNumber")
	countryCode := r.URL.Query().Get("countryCode")

	if phoneNumber == "" {
		http.Error(w, `{"error": "phoneNumber is required"}`, http.StatusBadRequest)
		return
	}

	response := ValidatePhoneNumber(phoneNumber, countryCode)

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(response)
}
