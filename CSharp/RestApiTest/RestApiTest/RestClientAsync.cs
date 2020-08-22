using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestApiTest
{
    public class RestClientAsync
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

        /// <summary>
        /// element of collection for Dealer -> Vehicles relationship
        /// </summary>
        private class DealerInternal
        {
            public int dealerId;
            public string name;
            private BlockingCollection<VehicleAnswer> _vehicles;
            public BlockingCollection<VehicleAnswer> Vehicles
            {
                get
                {
                    lock (this)
                    {
                        if (_vehicles == null)
                            _vehicles = new BlockingCollection<VehicleAnswer>();
                    }
                    return _vehicles;
                }
            }
        }

        /// <summary>
        /// constructor reading URL from app.config or web.config
        /// </summary>
        public RestClientAsync() : this(ConfigurationManager.AppSettings["endPoint"])
        {
        }

        /// <summary>
        /// universal constructor
        /// </summary>
        /// <param name="apiUrl">API Url</param>
        public RestClientAsync(string apiUrl)
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
            Debug.WriteLine("Start PostAnswerAsync");
            long startTicks = DateTime.UtcNow.Ticks;

            HttpResponseMessage response = await client.PostAsJsonAsync(String.Format(Constants.POST_ANSWER_URL, datasetId), answer);
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"PostAnswerAsync: status code {response.StatusCode}");

            var answerResponse = await response.Content.ReadAsAsync<AnswerResponse>();

            long elapsed = (DateTime.UtcNow.Ticks - startTicks) / 10000;
            Debug.WriteLine($"Finish PostAnswerAsync - elapsed: {elapsed}");

            return answerResponse;
        }

        /// <summary>
        /// Retrives dataset id (this method starts interation with API)
        /// </summary>
        /// <returns>Dataset object (contains the ID)</returns>
        public async Task<DatasetIdResponse> GetDatasetAsync()
        {
            Debug.WriteLine("Start GetDatasetAsync");
            long startTicks = DateTime.UtcNow.Ticks;

            HttpResponseMessage response = await client.GetAsync(Constants.GET_DATASET_URL);
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GetDatasetAsync: status code {response.StatusCode}");

            var dataset = await response.Content.ReadAsAsync<DatasetIdResponse>();

            long elapsed = (DateTime.UtcNow.Ticks - startTicks) / 10000;
            Debug.WriteLine($"Finish GetDatasetAsync - elapsed: {elapsed}");

            return dataset;
        }

        /// <summary>
        /// Test method for retrieval of "etalon" data
        /// </summary>
        /// <param name="datasetId">Dataset Id</param>
        /// <returns>Sample data</returns>
        public async Task<Answer> GetCheatAsync(string datasetId)
        {
            Debug.WriteLine("Start GetCheatAsync");
            long startTicks = DateTime.UtcNow.Ticks;

            HttpResponseMessage response = await client.GetAsync(String.Format(Constants.GET_CHEAT_URL, datasetId));
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GetCheatAsync: status code {response.StatusCode}");

            var answer = await response.Content.ReadAsAsync<Answer>();

            long elapsed = (DateTime.UtcNow.Ticks - startTicks) / 10000;
            Debug.WriteLine($"Finish GetCheatAsync - elapsed: {elapsed}");

            return answer;
        }

        /// <summary>
        /// Retrievs Vehicle Id collection
        /// </summary>
        /// <param name="datasetId">Dataset Id</param>
        /// <returns>Collection of IDs</returns>
        public async Task<VehiclesResponse> GetVehiclesAsync(string datasetId)
        {
            Debug.WriteLine("Start GetVehiclesAsync");
            long startTicks = DateTime.UtcNow.Ticks;

            HttpResponseMessage response = await client.GetAsync(String.Format(Constants.GET_VEHICLES_URL, datasetId));
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GetVehiclesAsync: status code {response.StatusCode}");

            var vehiclesResponse = await response.Content.ReadAsAsync<VehiclesResponse>();

            long elapsed = (DateTime.UtcNow.Ticks - startTicks) / 10000;
            Debug.WriteLine($"Finish GetVehiclesAsync - elapsed: {elapsed}");

            return vehiclesResponse;
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
                Debug.WriteLine($"Start GetVehicleAsync {vehicleId}");
                long startTicks = DateTime.UtcNow.Ticks;

                HttpResponseMessage response = await client.GetAsync(String.Format(Constants.GET_VEHICLE_URL, datasetId, vehicleId));

                if (!response.IsSuccessStatusCode)
                    throw new ApiException($"GetVehicleAsync: status code {response.StatusCode}");

                VehicleResponse vehicleResponse = await response.Content.ReadAsAsync<VehicleResponse>();

                VehicleAnswer vehicle = new VehicleAnswer()
                {
                    vehicleId = vehicleResponse.vehicleId,
                    year = vehicleResponse.year,
                    make = vehicleResponse.make,
                    model = vehicleResponse.model
                };

                await GetDealerAsync(datasetId, vehicleResponse.dealerId);

                DealersCollection[vehicleResponse.dealerId].Vehicles.Add(vehicle);

                long elapsed = (DateTime.UtcNow.Ticks - startTicks) / 10000;
                Debug.WriteLine($"Finish GetVehicleAsync {vehicleId} - elapsed: {elapsed}");

                return vehicleResponse;
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

                Debug.WriteLine($"Start GetDealerAsync {dealerId}");
                long startTicks = DateTime.UtcNow.Ticks;

                // reserve dealer id for processing by current task (if two tasks are trying to deal with the same dealerId)
                DealersCollection[dealerId] = new DealerInternal();

                HttpResponseMessage response = await client.GetAsync(String.Format(Constants.GET_DEALER_URL, datasetId, dealerId));

                if (!response.IsSuccessStatusCode)
                    throw new ApiException($"GetDealerAsync: status code {response.StatusCode}");

                DealerResponse dealer = await response.Content.ReadAsAsync<DealerResponse>();
                DealersCollection[dealerId].dealerId = dealer.dealerId;
                DealersCollection[dealerId].name = dealer.name;

                long elapsed = (DateTime.UtcNow.Ticks - startTicks) / 10000;
                Debug.WriteLine($"Finish GetDealerAsync {dealerId} - elapsed: {elapsed}");

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
                DatasetIdResponse dataset = await GetDatasetAsync();
                VehiclesResponse vehicles = await GetVehiclesAsync(dataset.datasetId);

                List<Task> tasks = new List<Task>();
                foreach (int vehicleId in vehicles.vehicleIds)
                    tasks.Add(Task.Run(() => GetVehicleAsync(dataset.datasetId, vehicleId)));

                await Task.WhenAll(tasks);

                Answer answer = ConvertToAnswer();

                // Post the answer
                AnswerResponse answerResponse = await PostAnswerAsync(dataset.datasetId, answer);
                return answerResponse;
            }
            catch (Exception e)
            {
                throw new ApiException(e.Message, e.InnerException);
            }
        }

        public async Task<AnswerResponse> RunDataSetAsync(string datasetId)
        {
            try
            {
                VehiclesResponse vehicles = await GetVehiclesAsync(datasetId);

                List<Task> tasks = new List<Task>();
                foreach (int vehicleId in vehicles.vehicleIds)
                    tasks.Add(Task.Run(() => GetVehicleAsync(datasetId, vehicleId)));

                await Task.WhenAll(tasks);

                Answer answer = ConvertToAnswer();

                // Post the answer
                AnswerResponse answerResponse = await PostAnswerAsync(datasetId, answer);
                return answerResponse;
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
            Answer answer = new Answer();
            int dealerIndex = 0;
            answer.dealers = new DealerAnswer[DealersCollection.Count];
            foreach (var dealer in DealersCollection)
            {
                DealerInternal d = dealer.Value;
                answer.dealers[dealerIndex] = new DealerAnswer
                {
                    dealerId = d.dealerId,
                    name = d.name,
                    vehicles = d.Vehicles.ToArray()
                };
                dealerIndex++;
            }

            return answer;
        }
    }
}
