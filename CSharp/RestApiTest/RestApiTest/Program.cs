using System;

namespace RestApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Async implementation:");
            var client = new RestClientAsync();
            var response = client.RunAsync().GetAwaiter().GetResult();

            //var simpleClient = new SimpleClient(apiUrl);
            //var datasetId = simpleClient.GetDatasetId();
            //var result = simpleClient.GetResult(datasetId);

            Console.WriteLine($"{response.success}, {response.message}, {response.totalMilliseconds}");

            Console.ReadLine();
        }
    }
}
