using System.Reflection.Metadata;
using Life.Board;

namespace Life.Runner
{
    public class Runner
    {
        const int DEFAULT_SIZE = 8;

        public static void Main()
        {
            var b = new LifeBoard(DEFAULT_SIZE, DEFAULT_SIZE * 2);

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(b);
                Console.WriteLine();
                b.Tick();
            }
        }
    }
}