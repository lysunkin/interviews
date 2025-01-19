## Description

This example shows a simple REST API server validating phone numbers

To build/test/run on Windows:

To compile:

`go build main.go`

To unit test:

```
cd api
go test
cd ..
```

To run:

`main.exe`

To build/test/run on Linux/MacOS the following commands can be run respectively:

```
make bin
make test
make run
```

Can be manually tested in a web browser as:
`http://localhost:8080/v1/phone-numbers?phoneNumber=%2B17373335066`

Which will result in the following response:
```
{
    "phoneNumber": "+17373335066",
    "countryCode": "US",
    "areaCode": "737",
    "localPhoneNumber": "3335066"
}
```

I decided to use this library https://github.com/dongri/phonenumber for a few reasons:
 - there are so many countries and it will be difficult to implement complete ISO 3166-1 mapping
 - manual implementation of all possible phone formats according to country standards is impossible witin given timeframe

NOTES:
I'd rather build everything as a Docker image to run it within isolated container.
Also, I think it would be better to perform build and test tasks also within a Docker container (to be platform-agnostic and don't rely on Windows/Linux/MacOS).
But that was not in a task description.
