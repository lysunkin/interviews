using System;
using System.Collections.Generic;

namespace TaskSolution
{
    public class ConsoleDataPresenter : IDataPresenter
    {
        public void SetCharactersFrequency(IEnumerable<KeyValuePair<char, int>> items)
        {
            Console.WriteLine("Characters frequency:");
            int currentItem = 0;
            foreach (KeyValuePair<char, int> item in items)
            {
                currentItem++;
                Console.Write(item.Key + " -> " + item.Value + "\t");
                if (currentItem % 6 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine("\n================================");
        }

        public void SetLongest(IEnumerable<KeyValuePair<string, int>> items)
        {
            Console.Write("Longest: ");
            foreach (KeyValuePair<String, int> item in items)
                Console.Write(item.Key + " ");
            Console.WriteLine();
        }

        public void SetMostFrequent(IEnumerable<KeyValuePair<string, int>> items)
        {
            Console.WriteLine("Most frequently used words:");
            foreach (KeyValuePair<String, int> item in items)
                Console.WriteLine(item.Key + " -> " + item.Value);
        }

        public void SetShortest(IEnumerable<KeyValuePair<string, int>> items)
        {
            Console.Write("Shortest: ");
            foreach (KeyValuePair<String, int> item in items)
                Console.Write(item.Key + " ");
            Console.WriteLine();
        }

        public void SetTotalCharacters(long value)
        {
            Console.Write("Total characters: ");
            Console.WriteLine(value);
        }

        public void SetTotalWords(long value)
        {
            Console.WriteLine("\n================================");
            Console.Write("Total words: ");
            Console.WriteLine(value);
        }
    }
}
