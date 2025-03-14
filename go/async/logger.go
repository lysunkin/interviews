package main

import (
	"bytes"
	"fmt"
	"io"
	"os"
	"sync"
	"time"
)

var bufPool = sync.Pool{
	New: func() any {
		return new(bytes.Buffer)
	},
}

// Replaceable time function for testing
var timeNow = func() time.Time {
	return time.Now()
}

func Log(w io.Writer, key, val string) {
	b := bufPool.Get().(*bytes.Buffer)
	defer func() {
		// Avoid returning excessively large buffers to the pool
		if b.Cap() <= 1024 {
			bufPool.Put(b)
		}
	}()
	b.Reset()

	b.WriteString(timeNow().UTC().Format(time.RFC3339))
	b.WriteByte(' ')
	b.WriteString(key)
	b.WriteByte('=')
	b.WriteString(val)
	b.WriteByte('\n')

	// Ensure the writer is thread-safe or wrap it with a mutex if needed
	_, err := w.Write(b.Bytes())
	if err != nil {
		fmt.Fprintf(os.Stderr, "failed to write log: %v\n", err)
	}
}

func main() {
	// Example usage of Log
	Log(os.Stdout, "path", "/search?q=flowers")
}
