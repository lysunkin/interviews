#!/usr/bin/env python

"""count_pairs.py: count pairs of items having the difference k within array arr."""

def countSimple(k, arr):
  sum = 0
  for i in range(len(arr)):
    for j in range(len(arr)-i):
      if abs(arr[i]-arr[i+j]) == k:
        sum+=1
  return sum

def countDict(k, arr):
  freq_dict = {}
  for i in range(len(arr)):
    if arr[i] in freq_dict:
      freq_dict[arr[i]] += 1
    else:
      freq_dict[arr[i]] = 1
  
  count = 0
  for item in freq_dict:
    if abs(k - item) in freq_dict:
      count += freq_dict[abs(k - item)]
  
  return count

if __name__ == "__main__": 
  a = [7, 1, 2, 3, 4, 5, 6, 7, 3, 2, 4, 11, 25, 35, 40]
  k = 3
  
  print(countSimple(k, a))
  print(countDict(k, a))
