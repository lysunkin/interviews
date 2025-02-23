using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        Parallel.ForEach(numbers, number =>
        {
            int square = number * number;
            Console.WriteLine($"Square of {number} is {square}");
        });
    }
}
