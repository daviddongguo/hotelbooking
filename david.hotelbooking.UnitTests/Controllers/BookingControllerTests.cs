using david.hotelbooking.api.Controllers;
using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class BookingControllerTests
    {
        private BookingsController _controller;
        private Mock<IBookingService> _mockService;
        private IQueryable<Booking> _bookingsList;


        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IBookingService>();
            DateTime firstDay = DateTime.Now.Date + new TimeSpan(14, 0, 0);
            _bookingsList = new List<Booking>
            {
                new Booking { Id = 1, GuestId = 1, Guest = new Guest{ Id = 1, Name = "Alice" }, RoomId = 1, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(1).Date + new TimeSpan(10, 0, 0) },
            }.AsQueryable();
            _controller = new BookingsController(_mockService.Object);
        }


        [Test]
        public void GetAllBookings_WhenDatabaseIsEmpty_ReturnEmpty()
        {
            _mockService.Setup(s => s.GetAllBookings()).Returns(Task.FromResult(new List<Booking>().AsQueryable()));
            var response = _controller.GetAllBookings().GetAwaiter().GetResult();

            Assert.That(response.Data.Count == 0);
            Utilities.PrintOut(response);
        }

        [TestCase(3)]
        [TestCase(9999)]
        public void GetAllBookings_ReturnTrue(int roomId)
        {
            _bookingsList.ToList()[0].RoomId = roomId;
            _mockService.Setup(s => s.GetAllBookings()).Returns(Task.FromResult(_bookingsList));
            var response = _controller.GetAllBookings().GetAwaiter().GetResult();

            Assert.That(response.Data.Count >= 1);
            Assert.That(response.Data.FirstOrDefault().Resource, Is.EqualTo(roomId.ToString()));
            Utilities.PrintOut(response);
        }

        [TestCase(1, true)]
        [TestCase(3, false)]
        public void GetBookingById_(int bookingId, bool expected)
        {
            _mockService.Setup(s => s.GetBookingById(1)).Returns(Task.FromResult(_bookingsList.ToList()[0]));
            var response = _controller.GetBookingById(bookingId).GetAwaiter().GetResult();

            Assert.That(response.Data != null, Is.EqualTo(expected));
            Utilities.PrintOut(response);
        }


    }
}
