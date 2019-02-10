/// <summary>
/// Classes for Data Transfer Objects
/// </summary>
namespace RestApiTest
{
    public class Dataset
    {
        public string datasetId;
    }

    public class AnswerResponse
    {
        public bool success;
        public string message;
        public long totalMilliseconds;
    }

    public class DealerResponse
    {
        public int dealerId;
        public string name;
    }

    public class VehiclesResponse
    {
        public int[] vehicleIds;
    }

    public class VehicleResponse
    {
        public int vehicleId;
        public int year;
        public string make;
        public string model;
        public int dealerId;
    }

    public class Answer
    {
        public Dealer[] dealers;
    }

    public class Dealer
    {
        public int dealerId;
        public string name;
        public Vehicle[] vehicles;
    }

    public class Vehicle
    {
        public int vehicleId;
        public int year;
        public string make;
        public string model;
    }
}
