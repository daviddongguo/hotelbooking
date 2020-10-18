using david.hotelbooking.domain;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using MockQueryable.Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace david.hotelbooking.UnitTests.Services
{
    [TestFixture]
    public class BookingServiceLocalDbTest
    {
        private BookingService _service;
        [SetUp]
        public void OneTimeSetUp()
        {
            _service = new BookingService(new InMemoryDbContextFactory().GetBookingContext());
        }

        [Test]
        public void pass()
        {
            Assert.That(true);
        }

        [Test]
        public void GetAllRooms_ReturnsAllRooms()
        {
            var res = _service.GetAllRooms().GetAwaiter().GetResult();

            // Assert
            Utilities.PrintOut(res);
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count() >= 1);
        }

        [Test]
        public void GetAllBookings_ReturnsAllBookings()
        {
            var res = _service.GetAllBookings().GetAwaiter().GetResult();

            // Assert
            Utilities.PrintOut(res);
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count() >= 1);
        }

        [TestCase(null, null, true)]
        [TestCase(1, null, true)]
        [TestCase(null, "amelia@ho.t", true)]
        [TestCase(null, "amelia", true)]
        [TestCase(null, "AmElia", true)]
        [TestCase(-1, null, false)]
        [TestCase(null, "fake", false)]
        public void GetAllBookings_ReturnsExpectedBookings(int roomId, string emailOrName, bool expected)
        {
            var res = _service.GetAllBookings(roomId, emailOrName).GetAwaiter().GetResult();

            // Assert
            Utilities.PrintOut(res);
            Assert.That(res?.Count() >= 1, Is.EqualTo(expected));
        }

        [Test]
        public void Overlopping_NoOverlopping_ReturnsNull()
        {
            var booking = new Booking { RoomId = 1 };
            var bookings = new List<Booking>
                {
                    new Booking {RoomId = 3}
                };
            var mock = bookings.AsQueryable().BuildMockDbSet();

            Assert.That(_service.OverlappingBookingExist(null, null).GetAwaiter().GetResult(), Is.Null);
            Assert.That(_service.OverlappingBookingExist(booking, null).GetAwaiter().GetResult(), Is.Null);
            Assert.That(_service.OverlappingBookingExist(null, mock.Object).GetAwaiter().GetResult(), Is.Null);
            Assert.That(_service.OverlappingBookingExist(booking, mock.Object).GetAwaiter().GetResult(), Is.Null);
        }

        [TestCase(-1, 1, true)]
        [TestCase(-1, -1, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, true)]
        [TestCase(0, 1, true)]
        [TestCase(1, -1, true)]
        [TestCase(1, 0, true)]
        [TestCase(1, 1, true)]
        [TestCase(-10, -10, false)]
        [TestCase(10, 10, false)]
        public void Overlopping_WhenOverlapping_ReturnsExistedBookingR(int FromDateOff, int ToDateOff, bool expected)
        {
            var existingBooking = new Booking
            {
                RoomId = 1,
                FromDate = new DateTime(2020, 09, 09, 14, 0, 0),
                ToDate = new DateTime(2020, 09, 13, 10, 0, 0),
            };
            var bookings = new List<Booking>
            {
                existingBooking,
            }.AsQueryable();
            var mock = bookings.AsQueryable().BuildMockDbSet();
            var booking = new Booking
            {
                RoomId = 1,
                FromDate = existingBooking.FromDate.AddDays(FromDateOff),
                ToDate = existingBooking.ToDate.AddDays(ToDateOff),
            };

            var result = _service.OverlappingBookingExist(booking, mock.Object).GetAwaiter().GetResult();

            Assert.That(result != null, Is.EqualTo(expected));
        }



        [TestCase("Alice", true)]
        [TestCase("li", true)]
        [TestCase("fake", false)]
        [TestCase("", false)]
        [TestCase("   ", false)]
        public void GetGuestByName(string name, bool expectedResult)
        {
            var res = _service.GetGuestByName(name).GetAwaiter().GetResult();

            // Assert
            Console.WriteLine(res?.Count());
            Utilities.PrintOut(res);
            Assert.That(res?.Count() >= 1, Is.EqualTo(expectedResult));

        }

        [TestCase("Alice", true)]
        [TestCase("li", true)]
        [TestCase("fake", false)]
        [TestCase("", false)]
        [TestCase("   ", false)]
        public void GetGuestByEmail(string email, bool expectedResult)
        {
            var res = _service.GetGuestByEmail(email).GetAwaiter().GetResult();

            // Assert
            Console.WriteLine(res?.Count());
            Utilities.PrintOut(res);
            Assert.That(res?.Count() >= 1, Is.EqualTo(expectedResult));

        }

        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        [TestCase(898989, false)]
        public void GetGuestById(int id, bool expectedResult)
        {
            var res = _service.GetGuestById(id).GetAwaiter().GetResult();

            // Assert
            Console.WriteLine(res);
            Utilities.PrintOut(res);
            Assert.That(res?.Id == id, Is.EqualTo(expectedResult));

        }

        [TestCase("", false)]
        [TestCase("   ", false)]
        [TestCase("fake", false)]
        [TestCase("ava@ho.t", true)]
        public void IsEmailExisted(string email, bool expectedResult)
        {
            var res = _service.IsEmailExisted(email).GetAwaiter().GetResult();

            // Assert
            Assert.That(res, Is.EqualTo(expectedResult));

        }

        [TestCase("", false)]
        [TestCase("   ", false)]
        [TestCase("ava@ho.t", false)]
        [TestCase("fake@email.com", true)]
        public void AddGuest(string email, bool expectedResult)
        {
            var res = _service.AddGuest(new Guest { Email = email, Name = email }).GetAwaiter().GetResult();

            // Assert
            Assert.That(res?.Email == email, Is.EqualTo(expectedResult));
        }








    }
}
