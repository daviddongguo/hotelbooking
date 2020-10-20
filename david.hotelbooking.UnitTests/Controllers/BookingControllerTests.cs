using david.hotelbooking.api.Controllers;
using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Moq;
using NUnit.Framework;
using RestSharp.Serialization.Json;
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
        private Booking _firstBooking;
        private Booking _secondBooking;
        private readonly JsonDeserializer _serializer = new JsonDeserializer();


        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IBookingService>();
            DateTime firstDay = DateTime.Now.Date + new TimeSpan(14, 0, 0);
            _firstBooking =
                new Booking { Id = 1, GuestId = 1, Guest = new Guest { Id = 1, Name = "Alice" }, RoomId = 1, Room = new Room { Id = 1, RoomNumber = "801" }, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(1).Date + new TimeSpan(10, 0, 0) };
            _secondBooking =
                new Booking { Id = 0, GuestId = 2, Guest = new Guest { Id = 2, Name = "Alex" }, RoomId = 2, Room = new Room { Id = 2, RoomNumber = "222" }, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(1).Date + new TimeSpan(10, 0, 0) };

            _bookingsList = new List<Booking>
            {
                _firstBooking
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

        [TestCase("1", "Alice@ho.t", "2020-1-1", "2020-1-2")]
        public void AddBooking(string roomId, string guestEmail, string fromDateStr, string toDateStr)
        {
            var ev = new BookingEvent
            {                
                Resource= roomId,
                Text = guestEmail,
                Start = fromDateStr,
                End = toDateStr,
            };
            _mockService.Setup(s => s.GetGuestByEmail(guestEmail)).Returns(
                Task.FromResult(new Guest { Id = 1, Email = guestEmail }));
            _mockService.Setup(s => s.AddBooking(It.IsAny<Booking>())).Returns(
                Task.FromResult(new Booking { Guest = new Guest { Email = guestEmail } }));

            var response = _controller.AddBooking(ev).GetAwaiter().GetResult();

            var resultRoomId = response.Data.RoomId;
            Assert.That(response.Data.RoomId.ToString(), Is.EqualTo(roomId));

        }
    }

}
