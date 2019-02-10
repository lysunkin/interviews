using System;
using System.Collections.Generic;

namespace HappyNumberApp
{
    /// <summary>
    /// task: to identify if number is lucky or not
    /// lucky number if the sum of squared digits can finaly be equal to one
    /// for example:
    /// 7 -> 7*7 -> 49
    /// 49 -> 4*4 + 9*9 -> 97
    /// 97 -> 9*9 + 7*7 -> 130
    /// 130 -> 1*1 + 3*3 + 0*0 -> 10
    /// 10 -> 1*1 + 0*0 -> 1
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            foreach (int num in new int[] { 5, 7, 82, 145 })
            {
                Console.WriteLine("{0} is a happy number: {1}", num, IsHappy(num));
            }
        }

        static public bool IsHappy(int num)
        {
            int res = num;
            HashSet<int> history = new HashSet<int>();
            while (true)
            {
                res = GetSumOfSq(res);

                // trace intermediate sums
                Console.WriteLine("#trace: {0}", res);

                // reached the desired result
                if (res == 1)
                    return true;

                // if cycle is detected
                if (history.Contains(res))
                    return false;

                history.Add(res);
            }
        }

        private static int GetSumOfSq(int num)
        {
            int sum = 0;
            while (true)
            {
                int j = num % 10;
                num = num / 10;

                sum += j * j;

                if (num == 0)
                    break;
            }

            return sum;
        }
    }
}
