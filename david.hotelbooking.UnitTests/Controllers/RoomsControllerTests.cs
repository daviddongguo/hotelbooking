using david.hotelbooking.api.Controllers;
using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.UnitTests.Controllers
{
    [TestFixture]
    public class RoomsControllerTests
    {
        private RoomsController _controller;
        private Mock<IBookingService> _mockService;
        //private readonly JsonDeserializer _serializer = new JsonDeserializer();

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IBookingService>();
            _controller = new RoomsController(_mockService.Object);
        }


        [TestCase(200)]
        public void GetAllRooms_WhenDatabaseIsEmpty_ReturnEmpty(int expectedStatusCode)
        {
            // Arrange
            _mockService.Setup(s => s.GetAllRooms()).Returns(Task.FromResult((new List<Room>()).AsQueryable()));

            // Action
            var response = _controller.GetAllRooms().GetAwaiter().GetResult();
            var result = response.Result as ObjectResult;

            // Assert
            Assert.That(result.StatusCode == expectedStatusCode);
            Assert.That(response, Is.InstanceOf(typeof(ActionResult<ServiceResponse<List<Resource>>>)));
            var resultValue = result.Value as ServiceResponse<List<Resource>>;
            Assert.That(resultValue.Message, Is.Null);
            Assert.That(resultValue.Data.Count == 0);
            Utilities.PrintOut(result);
        }

        [TestCase(200, 1)]
        public void GetAllRooms_ReturnResource(int expectedStatusCode, int expectedDataCount)
        {
            // Arrange
            _mockService.Setup(s => s.GetAllRooms()).Returns(Task.FromResult((new List<Room>
                { new Room
                    {
                        RoomGroupId = 1,
                        RoomGroup = new RoomGroup { Id = 1, Name = "test"}
                    }
                }).AsQueryable()));

            // Action
            var response = _controller.GetAllRooms().GetAwaiter().GetResult();

            // Assert
            Assert.That(response, Is.InstanceOf(typeof(ActionResult<ServiceResponse<List<Resource>>>)));
            var result = response.Result as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
            var resultValue = result.Value as ServiceResponse<List<Resource>>;
            Assert.That(resultValue.Message, Is.Null);
            Assert.That(resultValue.Data.Count == expectedDataCount);
            Utilities.PrintOut(result);
        }

        [TestCase(404)]
        public void GetAllRooms_ReturnNotFound(int expectedStatusCode)
        {
            _mockService.Setup(s => s.GetAllRooms()).Returns(Task.FromResult((new List<Room>
                { new Room
                    {
                        RoomGroupId = 1,
                        // RoomGroup = new RoomGroup { Id = 1, Name = "test"}
                    }
                }).AsQueryable()));
            var response = _controller.GetAllRooms().GetAwaiter().GetResult();

            // Assert
            var result = response.Result as ObjectResult;
            Assert.That(result.StatusCode == expectedStatusCode);
            var resultValue = result.Value as ServiceResponse<List<Resource>>;
            Assert.That(resultValue.Message, Is.Not.Null);
            Utilities.PrintOut(result);
        }
    }

}
