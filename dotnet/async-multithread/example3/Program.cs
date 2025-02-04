using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(3000); // Cancel after 3 seconds

        try
        {
            await LongRunningTaskAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task was canceled.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    static async Task LongRunningTaskAsync(CancellationToken token)
    {
        for (int i = 0; i < 10; i++)
        {
            token.ThrowIfCancellationRequested();
            Console.WriteLine($"Working... {i + 1}");
            await Task.Delay(1000); // Simulate work
        }
    }
}
