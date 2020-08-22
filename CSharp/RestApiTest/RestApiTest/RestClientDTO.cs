/// <summary>
/// Classes for Data Transfer Objects (to be represented as JSON on serialization)
/// </summary>
namespace RestApiTest
{
    public class DatasetIdResponse
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
        public DealerAnswer[] dealers;
    }

    public class DealerAnswer
    {
        public int dealerId;
        public string name;
        public VehicleAnswer[] vehicles;
    }

    public class VehicleAnswer
    {
        public int vehicleId;
        public int year;
        public string make;
        public string model;
    }
}
