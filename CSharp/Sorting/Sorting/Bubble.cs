using System;

namespace Sorting
{
    public class Bubble : ISorting
    {
        public void Sort(IComparable[] elements)
        {
            int l = elements.Length;
            IComparable t = elements[0];

            for (int i = 0; i < l; i++)
            {
                for (int j = i + 1; j < l; j++)
                {
                    if (elements[i].CompareTo(elements[j]) > 0)
                    {
                        t = elements[i];
                        elements[i] = elements[j];
                        elements[j] = t;
                    }
                }
            }
        }
    }
}
