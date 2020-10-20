using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
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
        private readonly JsonDeserializer _serializer = new JsonDeserializer();

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

           }
}
