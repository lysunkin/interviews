# Set the binary output name
BINARY_NAME=phone-api

# Build the Go binary
bin:
	@echo "Building the Go binary..."
	go build -o $(BINARY_NAME) main.go

# Run the tests
test:
	@echo "Running tests..."
	cd api && go test && cd ..

# Run the compiled binary
run: bin
	@echo "Running the application..."
	./$(BINARY_NAME)
