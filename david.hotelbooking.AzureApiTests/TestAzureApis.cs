using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Collections.Generic;

namespace david.hotelbooking.ApiTests
{
    [TestFixture]
    public class ApiWithRemoteDbTests
    {
        private RestClient _client;
        //private readonly string baseUrl = "http://localhost:5000/";
        private readonly string baseUrl = "https://davidwuhotelbooking.azurewebsites.net/";
        //private readonly JsonDeserializer _serializer = new JsonDeserializer();

        [SetUp]
        public void SetUp()
        {
            _client = new RestClient(baseUrl);
            _client.AddHandler("application/json", () => new RestSharp.Serialization.Json.JsonSerializer());
            _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }

        [TestCase(200), Timeout(8000)]
        public void TestUsers(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("api/users", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert
            Assert.That((int)response.StatusCode == expectedstatusCode);
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
            System.Console.WriteLine(response.Content);
        }

        [TestCase(200), Timeout(8000)]
        public void GetAllRoomsApi_ReturnsAllRooms(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("api/rooms", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert
            Assert.That((int)response.StatusCode == expectedstatusCode);
            Assert.That(response.Content.Contains("children"));
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
            System.Console.WriteLine(response.Content);
        }

        [TestCase(200), Timeout(8000)]
        public void GetAllBookinsApi_ReturnsAllEvents(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("api/bookings", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert
            // Assert
            Assert.That((int)response.StatusCode == expectedstatusCode);
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
            System.Console.WriteLine(response.Content);
        }

        [TestCase("1", "Alice@ho.t", "2020-1-1", "2020-1-2", 201)]
        public void AddBooking(string roomId, string guestEmail, string fromDateStr, string toDateStr, int expectedStatusCode)
        {
            // Arrange
            var request = new RestRequest("api/bookings", Method.POST);
            request.AddJsonBody(new BookingEvent
            {
                Text = guestEmail,
                Start = fromDateStr,
                End = toDateStr,
                Resource = roomId,
            });

            // Action
            //var response = _client.ExecuteAsync<Dictionary<string, string>>(request).GetAwaiter().GetResult();
            var response = _client.ExecuteAsync<ServiceResponse<Booking>>(request).GetAwaiter().GetResult();

            // Assert
            Assert.That((int)response.StatusCode == expectedStatusCode);
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
            System.Console.WriteLine(response.Content);

            //var data = response.Data["data"];
            //var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(data); ;
            //var id = obj["id"];
            var id = response.Data.Data.Id;

            // Delete
            request = new RestRequest($"api/bookings/{id}", Method.DELETE);
            var response2 = _client.ExecuteAsync<ServiceResponse<int>>(request).GetAwaiter().GetResult();
            // Assert
            System.Console.WriteLine(response2.Content);

            //Assert.That(response2.Data.Data == id);
        }
    }
}
