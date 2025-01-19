package api

import (
	"testing"
)

func TestValidatePhoneNumber(t *testing.T) {
	// Define test cases
	testCases := []struct {
		description     string
		phoneNumber     string
		countryCode     string
		expectedNumber  string
		expectedCountry string
		expectedArea    string
		expectedError   string
	}{
		{
			description:     "Valid phone number",
			phoneNumber:     "+12125690123",
			countryCode:     "",
			expectedNumber:  "+12125690123",
			expectedCountry: "US",
			expectedArea:    "212",
			expectedError:   "",
		},
		{
			description:     "Valid phone number with spaces",
			phoneNumber:     "+52 631 3118150",
			countryCode:     "",
			expectedNumber:  "+526313118150",
			expectedCountry: "MX",
			expectedArea:    "631",
			expectedError:   "",
		},
		{
			description:     "Valid phone number w/o plus",
			phoneNumber:     "34915872200",
			countryCode:     "ES",
			expectedNumber:  "+34915872200",
			expectedCountry: "ES",
			expectedArea:    "915",
			expectedError:   "",
		},
		{
			description:     "Valid phone number w/o plus sign and country code",
			phoneNumber:     "12125690123",
			countryCode:     "",
			expectedNumber:  "+12125690123",
			expectedCountry: "US",
			expectedArea:    "212",
			expectedError:   "",
		},
		{
			description:     "Missing country code in phone number",
			phoneNumber:     "63131128150",
			countryCode:     "",
			expectedNumber:  "",
			expectedCountry: "",
			expectedArea:    "",
			expectedError:   "country code is required",
		},
		{
			description:     "Invalid phone number format",
			phoneNumber:     "351 21 094 2000",
			countryCode:     "",
			expectedNumber:  "",
			expectedCountry: "",
			expectedArea:    "",
			expectedError:   "invalid phone number format",
		},
		{
			description:     "Invalid country code",
			phoneNumber:     "+17373335066",
			countryCode:     "MX",
			expectedNumber:  "",
			expectedCountry: "",
			expectedArea:    "",
			expectedError:   "country code is invalid",
		},
	}

	// Run test cases
	for _, tc := range testCases {
		t.Run(tc.description, func(t *testing.T) {
			resp := ValidatePhoneNumber(tc.phoneNumber, tc.countryCode)

			if resp.PhoneNumber != tc.expectedNumber {
				t.Errorf("expected phoneNumber %v; got %v", tc.expectedNumber, resp.PhoneNumber)
			}

			if resp.CountryCode != tc.expectedCountry {
				t.Errorf("expected countryCode %v; got %v", tc.expectedCountry, resp.CountryCode)
			}

			if resp.AreaCode != tc.expectedArea {
				t.Errorf("expected areaCode %v; got %v", tc.expectedArea, resp.AreaCode)
			}

			if resp.Error != tc.expectedError {
				t.Errorf("expected error %v; got %v", tc.expectedError, resp.Error)
			}
		})
	}
}
