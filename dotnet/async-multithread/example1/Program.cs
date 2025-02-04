using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string[] urls = {
            "https://microsoft.com",
            "https://amazon.com",
            "https://google.com"
        };

        var startTime = DateTime.Now;

        var tasks = urls.Select(url => DownloadFileAsync(url)).ToArray();
        await Task.WhenAll(tasks);

        var endTime = DateTime.Now;
        Console.WriteLine($"Total time: {(endTime - startTime).TotalSeconds} seconds");
    }

    static async Task DownloadFileAsync(string url)
    {
        using (var client = new HttpClient())
        {
            var content = await client.GetStringAsync(url);
            Console.WriteLine($"Downloaded {url} (Length: {content.Length})");
        }
    }
}
