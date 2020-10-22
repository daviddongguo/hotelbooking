using david.hotelbooking.UnitTests.Services;
using NUnit.Framework;
using System.Linq;

namespace david.hotelbooking.domain.Services.Tests
{
    [TestFixture()]
    public class RemoteDbRoomserviceTests
    {
        private BookingService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new BookingService(new RemoteMySqlDbcontextFactory().GetBookingContext());
        }

        [Test()]
        public void GetAllRoomsTest()
        {
            var result = _service.GetAllRooms().GetAwaiter().GetResult();

            // Assert
            Utilities.PrintOut(result);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.LastOrDefault().Id >= 1);
        }
    }
}
