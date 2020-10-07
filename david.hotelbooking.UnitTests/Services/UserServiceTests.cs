using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.UnitTests.Services;
using NUnit.Framework;
using System.Linq;

namespace david.hotelbooking.domain.Services.Tests
{
    [TestFixture()]
    public class UserServiceTests
    {
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new UserService(new InMemoryDbContextFactory().GetUserContext());
        }

        [Test]
        public void GetAllUsers_whenCalled_ReturnAllUsers()
        {
            // Arrange
            // Act
            var result = _service.GetAllUsers().GetAwaiter().GetResult();

            // Assert
            System.Console.WriteLine(result.ToList());

            foreach (var item in result.ToList())
            {
                System.Console.WriteLine(($"{item.Id} : {item.Email }"));
            }
            Assert.That(result, Is.Not.Null);

        }

        [TestCase("unique@email.com")]
        [TestCase("uniquee@email.com")]
        [TestCase("uniqueee@email.com")]
        public void AddUser_WhenNameIsUnique_ReturnAddedUser(string emailstr)
        {
            // Arrange
            // Act
            var result = _service.AddUser(new User { Email = emailstr }).GetAwaiter().GetResult();

            // Assert
            System.Console.WriteLine($"{result.Id} : {result.Email }");
            Assert.That(result.Id >= 1);
            Assert.That(result.Email.Equals(emailstr));
        }

        [TestCase("existed@email.com")]
        public void AddUser_WhenNameIsExisted_ReturnNull(string emailStr)
        {
            // Arrange
            // Act
            var result = _service.AddUser(new User { Email = emailStr }).GetAwaiter().GetResult();
            var repeat = _service.AddUser(new User { Email = emailStr }).GetAwaiter().GetResult();

            // Assert
            System.Console.WriteLine($"{result.Id} : {result.Email }");
            Assert.That(repeat, Is.Null);
        }
    }
}