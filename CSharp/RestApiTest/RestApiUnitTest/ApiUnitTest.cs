using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestApiTest;

namespace RestApiUnitTest
{
    [TestClass]
    public class ApiUnitTest
    {
        const string url = "http://api.coxauto-interview.com/";

        [TestMethod]
        public void TestGetDataset()
        {
            var client = new RestClientAsync(url);
            var response = client.GetDatasetAsync().GetAwaiter().GetResult();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.datasetId);
            Assert.AreNotEqual("", response.datasetId);
        }

        [TestMethod]
        public void TestPostAnswer()
        {
            var client = new RestClientAsync(url);
            var ds = client.GetDatasetAsync().GetAwaiter().GetResult();
            var cheat = client.GetCheatAsync(ds.datasetId).GetAwaiter().GetResult();
            var response = client.PostAnswerAsync(ds.datasetId, cheat).GetAwaiter().GetResult();

            Assert.IsNotNull(response);
            Assert.IsTrue(response.success);
        }

        [TestMethod]
        public void TestGetCheat()
        {
            var client = new RestClientAsync(url);
            var ds = client.GetDatasetAsync().GetAwaiter().GetResult();
            var cheat = client.GetCheatAsync(ds.datasetId).GetAwaiter().GetResult();

            Assert.IsNotNull(cheat);
            Assert.IsNotNull(cheat.dealers);
            Assert.AreEqual(3, cheat.dealers.Length);
            Assert.IsNotNull(cheat.dealers[0].vehicles);
            Assert.AreEqual(3, cheat.dealers[0].vehicles.Length);
        }

        [TestMethod]
        public void TestGetDealers()
        {
            var client = new RestClientAsync(url);
            var ds = client.GetDatasetAsync().GetAwaiter().GetResult();
            var response = client.GetVehiclesAsync(ds.datasetId).GetAwaiter().GetResult();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.vehicleIds);
            Assert.AreNotEqual(0, response.vehicleIds.Length);

            var vehicle = client.GetVehicleAsync(ds.datasetId, response.vehicleIds[0]).GetAwaiter().GetResult();

            Assert.IsNotNull(vehicle);
            Assert.AreNotEqual(0, vehicle.dealerId);

            var dealer = client.GetDealerAsync(ds.datasetId, vehicle.dealerId).GetAwaiter().GetResult();

            Assert.IsNull(dealer); // it must be so, because we already called GetVehicle!
        }

        [TestMethod]
        public void TestGetVehicles()
        {
            var client = new RestClientAsync(url);
            var ds = client.GetDatasetAsync().GetAwaiter().GetResult();
            var response = client.GetVehiclesAsync(ds.datasetId).GetAwaiter().GetResult();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.vehicleIds);
            Assert.AreEqual(9, response.vehicleIds.Length);
        }

        [TestMethod]
        public void TestGetVehicle()
        {
            var client = new RestClientAsync(url);
            var ds = client.GetDatasetAsync().GetAwaiter().GetResult();
            var response = client.GetVehiclesAsync(ds.datasetId).GetAwaiter().GetResult();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.vehicleIds);
            Assert.AreNotEqual(0, response.vehicleIds.Length);

            var vehicle = client.GetVehicleAsync(ds.datasetId, response.vehicleIds[0]).GetAwaiter().GetResult();

            Assert.IsNotNull(vehicle);
            Assert.AreNotEqual(0, vehicle.dealerId);
        }
    }
}
