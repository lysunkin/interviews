/*
A simple example of how to create and manipulate sparse vectors in Go:
we have a collection of vectors that are sparse in the sense that most of their
components are zero. We store only the non-zero components in a map, where the
key is the index of the component and the value is the value of the component.
*/
package main

import "fmt"

type SparseVectors struct {
	X, Y, Z map[int]float64
	Size    int
}

func NewSparseVectors() *SparseVectors {
	return &SparseVectors{
		X: make(map[int]float64),
		Y: make(map[int]float64),
		Z: make(map[int]float64),
	}
}

func (s *SparseVectors) Add(x, y, z float64) {
	if x != 0.0 {
		s.X[s.Size] = x
	}
	if y != 0.0 {
		s.Y[s.Size] = y
	}
	if z != 0.0 {
		s.Z[s.Size] = z
	}
	s.Size++
}

func (s *SparseVectors) Get(index int) (float64, float64, float64) {
	if index < 0 || index >= s.Size {
		panic(fmt.Sprintf("Index out of bounds: %d", index))
	}
	x, y, z := 0.0, 0.0, 0.0
	if val, ok := s.X[index]; ok {
		x = val
	}
	if val, ok := s.Y[index]; ok {
		y = val
	}
	if val, ok := s.Z[index]; ok {
		z = val
	}
	return x, y, z
}

func (s *SparseVectors) SumAll() (float64, float64, float64) {
	var sumX, sumY, sumZ float64
	for _, v := range s.X {
		sumX += v
	}
	for _, v := range s.Y {
		sumY += v
	}
	for _, v := range s.Z {
		sumZ += v
	}
	return sumX, sumY, sumZ
}

func main() {
	sv := NewSparseVectors()
	sv.Add(1.0, 2.0, 3.0)
	sv.Add(1.0, 0.0, 0.0)
	sv.Add(4.0, 5.0, 6.0)
	sv.Add(0.0, 1.0, 0.0)
	sv.Add(7.0, 8.0, 9.0)
	sv.Add(0.0, 0.0, 1.0)

	fmt.Print("Get index 1: ")
	fmt.Println(sv.Get(1))
	fmt.Print("Sum all: ")
	fmt.Println(sv.SumAll())
}
