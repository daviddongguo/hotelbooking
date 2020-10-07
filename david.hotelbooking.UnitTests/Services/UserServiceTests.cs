using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.UnitTests;
using david.hotelbooking.UnitTests.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            var userResult = result.Select( r => new 
            {
                Name = r.Email,
                Roles = r.UserRoles.Select( rr => new
                {
                    Role = rr.Role.Name,
                    Permissions = rr.Role.RolePermissions.Select (rrr => new
                    {
                        Permission = rrr.Permission.Name
                    })
                })
            });
            // Assert
            System.Console.WriteLine(Utilities.PrettyJson(JsonSerializer.Serialize(userResult)));
            Assert.That(result.LastOrDefault().Id >= 2);
            Assert.That(result.FirstOrDefault()
                .UserRoles.FirstOrDefault()
                .Role.RolePermissions.FirstOrDefault()
                .Permission.Name, Is.Not.Empty);
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