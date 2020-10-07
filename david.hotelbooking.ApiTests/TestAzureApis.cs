using NUnit.Framework;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace david.hotelbooking.ApiTests
{
    [TestFixture]
    public class TestAzureApis
    {
        private IClient _client;
        private readonly string baseUsrl = "https://davidwuhotelbooking.azurewebsites.net/";
        [SetUp]
        public void SetUp()
        {
            _client = new RestClient(baseUsrl);
        }
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            var answer = 42;
            Assert.That(answer, Is.EqualTo(42), "Some useful error message");
        }
    }
}
