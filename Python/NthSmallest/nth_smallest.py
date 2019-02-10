import sys 
  
def nth_smallest(arr, l, r, k): 
    if (k > 0 and k <= r - l + 1): 
      
        pos = partition(arr, l, r) 
  
        if (pos - l == k - 1): 
            return arr[pos] 
        if (pos - l > k - 1):  
            return nth_smallest(arr, l, pos - 1, k) 
  
        return nth_smallest(arr, pos + 1, r, k - pos + l - 1) 
  
    return sys.maxsize 
  
def partition(arr, l, r): 
    x = arr[r] 
    i = l 
    for j in range(l, r): 
        if (arr[j] <= x): 
            arr[i], arr[j] = arr[j], arr[i] 
            i += 1
    arr[i], arr[r] = arr[r], arr[i] 
    return i 
  
if __name__ == "__main__": 
    arr = [13, 3, 6, 7, 1, 19, 27] 
    n = len(arr) 
    k = 5 
    print "%d-th smallest element is %d" % (k, nth_smallest(arr, 0, n - 1, k))
