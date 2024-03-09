#!/usr/bin/env python

"""count_pairs.py: count pairs of items having the difference k within array arr."""

import time
import random

# simple and naive method with O(n^2) complexity
def countSimple(k, arr):
  count = 0
  n = len(arr)
  for i in range(n):
    for j in range(i+1,n):
      if abs(arr[i]-arr[j]) == k:
        count+=1
  return count

# faster method with O(n) complexity
def countDict(k, arr):
  freq_dict = {}

  # populate dictionary with numbers frequency
  for i in range(len(arr)):
    if arr[i] in freq_dict:
      freq_dict[arr[i]] += 1
    else:
      freq_dict[arr[i]] = 1
  
  count = 0
  # look for candidates having a difference of k
  for key in freq_dict.keys():
    candidate = key+k
    if candidate in freq_dict:
      if k == 0:
        # special case for 0
        count += freq_dict[key]-1
      else:
        # number of permutations
        count += freq_dict[candidate] * freq_dict[key]
  
  return count

def execTime(prompt, f, k, arr):
  start = time.time()
  print(prompt, f(k, arr))
  end = time.time()
  print("Spent time (s):", end - start)

if __name__ == "__main__": 
  a = [7, 1, 2, 3, 4, 5, 6, 7, 3, 2, 4, 11, 25, 35, 40]
  k = 3

  print("Results for array", a, "and difference", k)
  print("Naive:", countSimple(k, a))
  print("Fast:", countDict(k, a))

  size = 10000
  a = []
  for i in range(size):
    a.append(random.randint(0, 1000))

  print("Results for random numbers array of size", size, "and difference", k)
  execTime("Naive:", countSimple, k, a)
  execTime("Fast:", countDict, k, a)
