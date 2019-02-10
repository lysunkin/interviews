using System;

namespace Sorting
{
    /// <summary>
    /// simple example of two sorting algorithms working through one interface
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string[] arr = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };

            Console.WriteLine("Original data:");
            Console.WriteLine(string.Join(",", arr));

            ISorting s1 = new Bubble();
            s1.Sort(arr);

            Console.WriteLine("After Bubble Sort:");
            Console.WriteLine(string.Join(",", arr));

            arr = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };

            ISorting s2 = new QSort();
            s2.Sort(arr);

            Console.WriteLine("After Quick Sort:");
            Console.WriteLine(string.Join(",", arr));
        }
    }
}
