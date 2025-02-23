using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var task = Task.Run(() => 10)
            .ContinueWith(t => t.Result * 2)
            .ContinueWith(t => Console.WriteLine($"Result: {t.Result}"));

        await task;
    }
}
