using david.hotelbooking.domain.Services;
using NUnit.Framework;
using System;

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





    }
}