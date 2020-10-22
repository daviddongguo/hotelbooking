using david.hotelbooking.api.Controllers;
using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RestSharp.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tests
{
    [TestFixture]
    public class RoomsControllerTests
    {
        private RoomsController _controller;
        private Mock<IBookingService> _mockService;
        private readonly JsonDeserializer _serializer = new JsonDeserializer();

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IBookingService>();
            _controller = new RoomsController(_mockService.Object);
        }


        [Test]
        public void GetAllRooms_WhenDatabaseIsEmpty_ReturnEmpty()
        {
            _mockService.Setup(s => s.GetAllRooms()).Returns(Task.FromResult((new List<Room>()).AsQueryable()));
            var response = _controller.GetAllRooms().GetAwaiter().GetResult();
            var result = response.Result as OkObjectResult;

            Utilities.PrintOut(result);
            Assert.That(response, Is.InstanceOf(typeof(ActionResult<ServiceResponse<List<Resource>>>)));
            Assert.That(result.StatusCode == 200);
            var output = result.Value as ServiceResponse<List<Resource>>;
            Assert.That(output.Data.Count == 0);
        }

        [Test]
        public void GetAllRooms_ReturnResource()
        {
            _mockService.Setup(s => s.GetAllRooms()).Returns(Task.FromResult((new List<Room>
                { new Room
                    {
                        RoomGroupId = 1,
                        RoomGroup = new RoomGroup { Id = 1, Name = "test"}
                    }
                }).AsQueryable()));
            var response = _controller.GetAllRooms().GetAwaiter().GetResult();
            var result = response.Result as OkObjectResult;

            Utilities.PrintOut(result);
            Assert.That(response, Is.InstanceOf(typeof(ActionResult<ServiceResponse<List<Resource>>>)));
            Assert.That(result.StatusCode == 200);
            var output = result.Value as ServiceResponse<List<Resource>>;
            Assert.That(output.Data.Count == 1);
        }

    }

}
