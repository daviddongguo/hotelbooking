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
        public void WhenCalledDefaultFromDate_ReturnNow14()
        {
            //
            var booking = new Booking();

            // Assert
            var fromdate = booking.FromDate;
            System.Console.WriteLine(fromdate);
            Assert.That(fromdate.Hour == 14);
        }

        [TestCase("2020.10.16")]
        [TestCase("2020.10.1")]
        public void WhenSetFromDate_Return14(string dateStr)
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
            Assert.That(fromdate.Hour == 14);

        }
        [TestCase("2020.1.10")]
        [TestCase("2020.10.1")]
        public void WhenSetToDate_Return10(string dateStr)
        {
            // Arrange
            var dateTime = DateTime.Parse(dateStr);
            var booking = new Booking
            {
                FromDate = dateTime.AddDays(-1),
                ToDate = dateTime,
            };

            // Assert
            var fromdate = booking.ToDate;
            System.Console.WriteLine(fromdate);
            Assert.That(fromdate.Hour == 10);
        }

        [TestCase("2020.01.10", "2020.01,01")]
        public void WhenSetWrongToDate_ReturnError(string fromDateStr, string toDateStr)
        {
            // Arrange
            var fromDate = DateTime.Parse(fromDateStr);
            var toDate = DateTime.Parse(toDateStr);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    var booking = new Booking
                    {
                        FromDate = fromDate,
                        ToDate = toDate
                    };
                });

        }

    }
}
