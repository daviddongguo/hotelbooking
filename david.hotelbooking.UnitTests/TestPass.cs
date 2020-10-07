using NUnit.Framework;

namespace david.hotelbooking.UnitTests
{
    public class Pass
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEverPass()
        {
            Assert.Pass();
        }
    }
}