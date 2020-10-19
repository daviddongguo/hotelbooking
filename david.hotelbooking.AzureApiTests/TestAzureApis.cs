using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace david.hotelbooking.ApiTests
{
    [TestFixture]
    public class TestAzureApis
    {
        private RestClient _client;
        private readonly string baseUrl = "https://davidwuhotelbooking.azurewebsites.net/";
        [OneTimeSetUp]
        public void SetUp()
        {
            _client = new RestClient(baseUrl);
            _client.AddHandler("application/json", () => new JsonSerializer());
        }
        [TestCase(200)]
        public void TestUsers(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("testusers", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert

            Assert.That((int)response.StatusCode == expectedstatusCode);
            System.Console.WriteLine(response.ResponseUri);
            Utilities.PrintOut(response.Content);
            Assert.That(response.Content, Is.Not.Null);
        }

        [TestCase("200")]
        public void GetAllRolePermission_ReturnsAll(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("api/rolepermission", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert

            Assert.That((int)response.StatusCode == expectedstatusCode);
            System.Console.WriteLine(response.ResponseUri);
            Utilities.PrintOut(response.Content);
            Assert.That(response.Content, Is.Not.Null);

        }

        [TestCase("200")]
        public void GetAllBookings_ReturnsAll(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("api/bookings", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert

            Assert.That((int)response.StatusCode == expectedstatusCode);
            System.Console.WriteLine(response.ResponseUri);
            Utilities.PrintOut(response.Content);
            Assert.That(response.Content, Is.Not.Null);

        }

        [TestCase("1", "Alice@ho.t", "2020-1-1", "2020-1-2", 201)]
        public void AddBooking(string roomId, string guestEmail, string fromDateStr, string toDateStr, int expectedstatusCode)
        {
            var ev = new BookingEvent
            {
                Id = roomId,
                Text = guestEmail,
                Start = fromDateStr,
                End = toDateStr,
            };

            // Arrange
            var request = new RestRequest("api/bookings", Method.POST);


            // Act

            // Assert

            Assert.That(true);


        }
    }
}
