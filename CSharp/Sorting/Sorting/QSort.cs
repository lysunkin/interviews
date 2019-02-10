using System;

namespace Sorting
{
    public class QSort : ISorting
    {
        public void Sort(IComparable[] elements)
        {
            Quicksort(elements, 0, elements.Length - 1);
        }

        private void Quicksort(IComparable[] elements, int left, int right)
        {
            int i = left, j = right;
            IComparable middle = elements[(left + right) / 2];

            while (i <= j)
            {
                while (elements[i].CompareTo(middle) < 0)
                    i++;

                while (elements[j].CompareTo(middle) > 0)
                    j--;

                if (i <= j)
                {
                    IComparable t = elements[i];
                    elements[i] = elements[j];
                    elements[j] = t;
                    i++;
                    j--;
                }
            }

            if (left < j)
                Quicksort(elements, left, j);

            if (i < right)
                Quicksort(elements, i, right);
        }
    }
}
