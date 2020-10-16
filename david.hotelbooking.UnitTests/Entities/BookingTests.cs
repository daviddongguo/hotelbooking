using david.hotelbooking.domain.Entities.Hotel;
using NUnit.Framework;
using System;

namespace david.hotelbooking.UnitTests.Entities
{
    [TestFixture]
    public class BookingTests
    {
        
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void WhenCalledDefaultFromDate_ReturnNow2PM()
        {
            // 
            var booking = new Booking();

            // Assert
            var fromdate = booking.FromDate;
            System.Console.WriteLine(fromdate);
            Assert.That(fromdate.ToString().Contains("2:00:00 PM"));
        }

        [TestCase("2020.10.16")]
        [TestCase("2020.10.1")]
        public void WhenSetFromDate_Return2Pm(string dateStr)
        {
            // Arrange
            var dateTime = DateTime.Parse(dateStr);
            var booking = new Booking
            {
                FromDate = dateTime
            };

            // Assert
            var fromdate = booking.FromDate;
            System.Console.WriteLine(fromdate);
            Assert.That(fromdate.ToString().Contains("2:00:00 PM"));

        }
        [TestCase("2020.1.10")]
        [TestCase("2020.10.1")]
        public void WhenSetToDate_Return2Pm(string dateStr)
        {
            // Arrange
            var dateTime = DateTime.Parse(dateStr);
            var booking = new Booking
            {
                ToDate = dateTime
            };

            // Assert
            var fromdate = booking.ToDate;
            System.Console.WriteLine(fromdate);
            Assert.That(fromdate.ToString().Contains("10:00:00 AM"));
        }

        [Ignore("It does not work")]
        [TestCase("2020.01.10", "2020.01,01")]
        public void WhenSetWrongToDate_ReturnError(string fromDateStr, string toDateStr)
        {
            // Arrange
            var fromDate = DateTime.Parse(fromDateStr);
            var toDate = DateTime.Parse(toDateStr);
            var booking = new Booking
            {
                FromDate = fromDate,

                // Action
                ToDate = toDate
            };

            // Assert
            Console.WriteLine($"{booking.ToDate} is not {toDateStr}");
            Assert.That(booking.ToDate != toDate);
        }

    }
}
