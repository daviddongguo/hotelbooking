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
    public class UserServiceLocalInMemoryDbTest
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
            var userResults = result.Select( r => new 
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
            System.Console.WriteLine(Utilities.PrettyJson(JsonSerializer.Serialize(userResults)));
            Assert.That(result.LastOrDefault().Id >= 2);
            Assert.That(result.FirstOrDefault()
                .UserRoles.FirstOrDefault()
                .Role.RolePermissions.FirstOrDefault()
                .Permission.Name, Is.Not.Empty);
        }

        [TestCase("admin@hotel.com", true)]
        [TestCase("ADMIN@hotel.com", true)]
        [TestCase("xxxxx@hotel.com", false)]
        public void GetSingleUser_WhenCalled_RetunUserOrNull(string emailStr, bool expectedResult)
        {
            var result = _service.GetSingleUser(emailStr).GetAwaiter().GetResult();

            Assert.That(result?.Email != null, Is.EqualTo(expectedResult));
        }

        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        public void GetSingleUser_WhenCalled_RetunUserOrNull(int id, bool expectedResult)
        {
            var result = _service.GetSingleUser(id).GetAwaiter().GetResult();

            Assert.That(result?.Id != null, Is.EqualTo(expectedResult));
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