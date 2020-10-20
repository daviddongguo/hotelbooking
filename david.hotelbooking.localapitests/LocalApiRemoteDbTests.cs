using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace david.hotelbooking.UnitTests.Apis
{
    [TestFixture]
    public class LocalApiRemoteDbTests
    {
        private RestClient _client;
        private readonly string baseUrl = "http://localhost:5000/";
        private readonly JsonDeserializer _serializer = new JsonDeserializer();

        [OneTimeSetUp]
        public void SetUp()
        {
            _client = new RestClient(baseUrl);
            _client.AddHandler("application/json", () => new JsonSerializer());
            _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }

        [TestCase(200), Timeout(2000)]
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
            Utilities.PrintOut(response.Content);
            Assert.That(response.Content, Is.Not.Null);
        }

        [TestCase(200), Timeout(2000)]
        public void GetAllRoomsApi_ReturnsAllRooms(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("api/rooms", Method.GET);


            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert

            Assert.That((int)response.StatusCode == expectedstatusCode);
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
            Utilities.PrintOut(response.Content);
            Assert.That(response.Content, Is.Not.Null);
        }

        [TestCase(200), Timeout(2000)]
        public void GetAllBookinsApi_ReturnsAllBookings(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("api/bookings", Method.GET);


            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert

            Assert.That((int)response.StatusCode == expectedstatusCode);
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
            Utilities.PrintOut(response.Content);
            Assert.That(response.Content, Is.Not.Null);
        }

        [TestCase("1", "Alice@ho.t", "2020-1-1", "2020-1-2", true)]
        public void AddBooking(string roomId, string guestEmail, string fromDateStr, string toDateStr, bool expected)
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
            var response = _client.ExecuteAsync(request).GetAwaiter().GetResult();

            // Assert
            var result = _serializer.Deserialize<ServiceResponse<Booking>>(response);
            Utilities.PrintOut(result);


            Assert.That(result.Success);
            Assert.That(result.Data.Id > 1);
            var id = result.Data.Id;

            // Delete
            request = new RestRequest($"api/bookings/{id}", Method.DELETE);
            response = _client.ExecuteAsync(request).GetAwaiter().GetResult();
            Utilities.PrintOut(response.Content);
        }

    }

}
