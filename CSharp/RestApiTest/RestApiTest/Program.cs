using System;

namespace RestApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient();
            var response = client.RunAsync().GetAwaiter().GetResult();
            Console.WriteLine($"{response.success}, {response.message}, {response.totalMilliseconds}");

            Console.ReadLine();
        }
    }
}
