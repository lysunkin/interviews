using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestApiTest
{
    public class RestClient
    {
        private HttpClient client;

        private ConcurrentDictionary<int, DealerInternal> _dealers;

        /// <summary>
        /// accessor for internal storage with lazy intitialization
        /// </summary>
        private ConcurrentDictionary<int, DealerInternal> DealersCollection
        {
            get
            {
                lock(this)
                {
                    if (_dealers == null)
                        _dealers = new ConcurrentDictionary<int, DealerInternal>();
                }
                return _dealers;
            }
        }

        [Obsolete("This method is for testing only!", false)]
        public string GetDealerNameById(int id)
        {
            var di = DealersCollection[id];
            if (di != null)
                return di.name;
            else
                return null;
        }

        /// <summary>
        /// element of collection for Dealer -> Vehicles relationship
        /// </summary>
        private class DealerInternal
        {
            public int dealerId;
            public string name;
            private BlockingCollection<Vehicle> _vehicles;
            public BlockingCollection<Vehicle> Vehicles
            {
                get
                {
                    lock (this)
                    {
                        if (_vehicles == null)
                            _vehicles = new BlockingCollection<Vehicle>();
                    }
                    return _vehicles;
                }
            }
        }

        /// <summary>
        /// constructor reading URL from app.config or web.config
        /// </summary>
        public RestClient() : this(ConfigurationManager.AppSettings["endPoint"])
        {
        }

        /// <summary>
        /// universal constructor
        /// </summary>
        /// <param name="apiUrl">API Url</param>
        public RestClient(string apiUrl)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(apiUrl)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Submits Answer
        /// </summary>
        /// <param name="datasetId">Dataset Id</param>
        /// <param name="answer">Answer object</param>
        /// <returns>result of evaluation</returns>
        public async Task<AnswerResponse> PostAnswerAsync(string datasetId, Answer answer)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"api/{datasetId}/answer", answer);
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"PostAnswerAsync: status code {response.StatusCode}");

            return await response.Content.ReadAsAsync<AnswerResponse>();
        }

        /// <summary>
        /// Retrives dataset id (this method starts interation with API)
        /// </summary>
        /// <returns>Dataset object (contains the ID)</returns>
        public async Task<Dataset> GetDatasetAsync()
        {
            HttpResponseMessage response = await client.GetAsync("api/datasetId");
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GetDatasetAsync: status code {response.StatusCode}");

            return await response.Content.ReadAsAsync<Dataset>();
        }

        /// <summary>
        /// Test method for retrieval of "etalon" data
        /// </summary>
        /// <param name="datasetId">Dataset Id</param>
        /// <returns>Sample data</returns>
        public async Task<Answer> GetCheatAsync(string datasetId)
        {
            HttpResponseMessage response = await client.GetAsync($"api/{datasetId}/cheat");
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GetCheatAsync: status code {response.StatusCode}");

            return await response.Content.ReadAsAsync<Answer>();
        }

        /// <summary>
        /// Retrievs Vehicle Id collection
        /// </summary>
        /// <param name="datasetId">Dataset Id</param>
        /// <returns>Collection of IDs</returns>
        public async Task<VehiclesResponse> GetVehiclesAsync(string datasetId)
        {
            HttpResponseMessage response = await client.GetAsync($"api/{datasetId}/vehicles");
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GetVehiclesAsync: status code {response.StatusCode}");

            return await response.Content.ReadAsAsync<VehiclesResponse>();
        }

        /// <summary>
        /// Retrieves Vehicle information, also retrieves connected Dealer (for optimal and parallel execution)
        /// </summary>
        /// <param name="datasetId">Dataset Id</param>
        /// <param name="vehicleId">Vehicle Id</param>
        /// <returns>Vehicle object</returns>
        public async Task<VehicleResponse> GetVehicleAsync(string datasetId, int vehicleId)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"api/{datasetId}/vehicles/{vehicleId}");

                if (!response.IsSuccessStatusCode)
                    throw new ApiException($"GetVehicleAsync: status code {response.StatusCode}");

                VehicleResponse vehicle = await response.Content.ReadAsAsync<VehicleResponse>();

                Vehicle v = new Vehicle()
                {
                    vehicleId = vehicle.vehicleId,
                    year = vehicle.year,
                    make = vehicle.make,
                    model = vehicle.model
                };

                await GetDealerAsync(datasetId, vehicle.dealerId);

                DealersCollection[vehicle.dealerId].Vehicles.Add(v);

                return vehicle;
            }
            catch (Exception e)
            {
                throw new ApiException($"GetVehicleAsync: {e.Message}", e.InnerException);
            }
        }

        /// <summary>
        /// Retrieves Dealer, also used for optimized data retrieval,
        /// where internal data storage is populated and if the process of retrieval initiated then return null
        /// </summary>
        /// <param name="datasetId">Dataset Id</param>
        /// <param name="dealerId">Dealed Id</param>
        /// <returns>Dealer object or null</returns>
        public async Task<DealerResponse> GetDealerAsync(string datasetId, int dealerId)
        {
            try
            {
                if (DealersCollection.ContainsKey(dealerId))
                    return null;

                DealersCollection[dealerId] = new DealerInternal();

                HttpResponseMessage response = await client.GetAsync($"api/{datasetId}/dealers/{dealerId}");

                if (!response.IsSuccessStatusCode)
                    throw new ApiException($"GetDealerAsync: status code {response.StatusCode}");

                DealerResponse dealer = await response.Content.ReadAsAsync<DealerResponse>();
                DealersCollection[dealer.dealerId].dealerId = dealer.dealerId;
                DealersCollection[dealer.dealerId].name = dealer.name;

                return dealer;
            }
            catch (Exception e)
            {
                throw new ApiException($"GetDealerAsync: {e.Message}", e.InnerException);
            }
        }

        /// <summary>
        /// Running the main workflow of data transformation and exchange
        /// </summary>
        /// <returns>result of data posting</returns>
        public async Task<AnswerResponse> RunAsync()
        {
            try
            {
                Dataset ds = await GetDatasetAsync();
                Answer cheat = await GetCheatAsync(ds.datasetId);
                VehiclesResponse vs = await GetVehiclesAsync(ds.datasetId);

                List<Task> tasks = new List<Task>();
                foreach (int vId in vs.vehicleIds)
                    tasks.Add(Task.Run(() => GetVehicleAsync(ds.datasetId, vId)));

                await Task.WhenAll(tasks);

                Answer a = ConvertToAnswer();

                // Post the answer
                AnswerResponse ar = await PostAnswerAsync(ds.datasetId, a);
                return ar;
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// Creates Answer object from internal object storage
        /// </summary>
        /// <returns>Answer object</returns>
        private Answer ConvertToAnswer()
        {
            Answer a = new Answer();
            int iDealer = 0;
            a.dealers = new Dealer[DealersCollection.Count];
            foreach (var dDi in DealersCollection)
            {
                DealerInternal d = dDi.Value;
                a.dealers[iDealer] = new Dealer
                {
                    dealerId = d.dealerId,
                    name = d.name,
                    vehicles = d.Vehicles.ToArray()
                };
                iDealer++;
            }

            return a;
        }
    }
}
