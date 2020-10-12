using NUnit.Framework;
using RestSharp;
using RestSharp.Serialization.Json;

namespace david.hotelbooking.ApiTests
{
    [TestFixture]
    public class TestAzureApis
    {
        private RestClient _client;
        private readonly string baseUsrl = "https://davidwuhotelbooking.azurewebsites.net/";
        [OneTimeSetUp]
        public void SetUp()
        {
            _client = new RestClient(baseUsrl);
            _client.AddHandler("application/json", () => new JsonSerializer());
        }
        [TestCase(200)]
        public void TestUsers(int expectedstatusCoode)
        {
            // Arrange
            var request = new RestRequest("testusers", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert

            Assert.That((int)response.StatusCode == expectedstatusCode);
            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(Utilities.PrettyJson(response.Content));
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
            System.Console.WriteLine(Utilities.PrettyJson(response.Content));
            Assert.That(response.Content, Is.Not.Null);

        }
    }
}
