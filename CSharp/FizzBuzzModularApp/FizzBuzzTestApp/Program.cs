using System;
using TestClassLibrary;

namespace FizzBuzzTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            FizzBuzzClass fb = new FizzBuzzClass("Foo", "Bar");

            for (int i = 1; i < 101; i++)
            {
                Console.WriteLine(fb.GetFizzBuzz(i));
            }
        }
    }
}
