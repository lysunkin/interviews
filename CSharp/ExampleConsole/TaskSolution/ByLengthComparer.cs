using System;
using System.Collections.Generic;

namespace TaskSolution
{
    // comparer for a collection in order to implement sorting by the length of the words
    // if the length is the same then case insensitive alphabetical ordering is used
    public class ByLengthComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x.Length == y.Length)
                return String.Compare(x.ToLower(), y.ToLower());
            if (x.Length > y.Length)
                return 1;
            else
                return -1;
        }
    }
}
