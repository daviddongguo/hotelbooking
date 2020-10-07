using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.UnitTests.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace david.hotelbooking.domain.Services.Tests
{
    [TestFixture()]
    public class UserServiceTests
    {
        private UserDbContext inMemoryDbContext;

        [OneTimeSetUp]
        public void OnetimeSetUP()
        {
            inMemoryDbContext = new InMemoryDbContextFactory().GetUserContext();
        }
        [Test()]
        public void AddUser_WhenNameIsUnique_ReturnAddedUser()
        {
            // Arrange
            var service = new UserService(inMemoryDbContext);
            string expectedEcmail = "a@bc";

            // Act
            var result = service.AddUser(new User { Email = expectedEcmail }).GetAwaiter().GetResult();

            // Assert
            System.Console.WriteLine($"{result.Id} : {result.Email }");
            Assert.That(result.Id >= 1);
            Assert.That(result.Email.Equals(expectedEcmail));
        }
    }
}