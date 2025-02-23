using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        await DoWorkAsync();
    }

    static async Task DoWorkAsync()
    {
        await Task.Delay(1000).ConfigureAwait(false);
        Console.WriteLine("Work done.");
    }
}
