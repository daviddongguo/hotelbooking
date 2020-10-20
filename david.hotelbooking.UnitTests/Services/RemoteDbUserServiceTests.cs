using david.hotelbooking.UnitTests;
using david.hotelbooking.UnitTests.Services;
using NUnit.Framework;
using System.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace david.hotelbooking.domain.Services.Tests
{
    [TestFixture()]
    public class RemoteDbUserServiceTests
    {
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            //_service = new UserService(new InMemoryDbContextFactory().GetUserContext());
            _service = new UserService(new RemoteMySqlDbcontextFactory().GetUserContext());
        }

        [Test]
        public void GetAllUsers_whenCalled_ReturnAllUsers()
        {
            // Arrange
            // Act
            var result = _service.GetAllUsers().GetAwaiter().GetResult();
            var userResults = result.Select(r => new
            {
                Name = r.Email,
                Roles = r.UserRoles.Select(rr => new
                {
                    Role = rr.Role.Name,
                    Permissions = rr.Role.RolePermissions.Select(rrr => new
                    {
                        Permission = rrr.Permission.Name
                    })
                })
            });
            // Assert
            Utilities.PrintOut(userResults);
            Assert.That(result.LastOrDefault().Id >= 2);
            Assert.That(result.FirstOrDefault()
                .UserRoles.FirstOrDefault()
                .Role.RolePermissions.FirstOrDefault()
                .Permission.Name, Is.Not.Empty);
        }

    }
}
