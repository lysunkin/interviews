using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var queue = new ConcurrentQueue<int>();

        var tasks = new Task[10];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < 10; j++)
                {
                    queue.Enqueue(j);
                }
            });
        }

        Task.WaitAll(tasks);

        Console.WriteLine($"Total items in queue: {queue.Count}");
    }
}
