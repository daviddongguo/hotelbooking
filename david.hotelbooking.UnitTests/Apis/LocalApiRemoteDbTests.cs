using david.hotelbooking.domain;
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
        [OneTimeSetUp]
        public void SetUp()
        {
            _client = new RestClient(baseUrl);
            _client.AddHandler("application/json", () => new JsonSerializer());
            _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }
        [TestCase(200)]
        public void TestUsers(int expectedstatusCode)
        {
            // Arrange
            var request = new RestRequest("api/users", Method.GET);

            // Act
            var response = _client.ExecuteGetAsync(request).GetAwaiter().GetResult();

            // Assert

            //Assert.That((int)response.StatusCode == expectedstatusCode);

            System.Console.WriteLine(response.ResponseUri);
            System.Console.WriteLine(response.StatusCode);
            System.Console.WriteLine(Utilities.PrettyJson(response.Content));
            Assert.That(response.Content, Is.Not.Null);
        }
    }

}
