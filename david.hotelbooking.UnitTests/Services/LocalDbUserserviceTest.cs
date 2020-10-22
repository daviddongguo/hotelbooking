using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.UnitTests.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace david.hotelbooking.domain.Services.Tests
{
    [TestFixture]
    public class LocalDbUserserviceTest
    {
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new UserService(new LocalInMemoryDbContextFactory().GetUserContext());
        }

        [Test]
        public void GetAllUsers_whenCalled_ReturnAllUsers()
        {
            // Act
            var result = _service.GetAllUsers().GetAwaiter().GetResult();

            // Assert
            PrintOutUsers(result);
            Assert.That(result.LastOrDefault().Id >= 2);
            Assert.That(result.FirstOrDefault()
                .UserRoles.FirstOrDefault()
                .Role.RolePermissions.FirstOrDefault()
                .Permission.Name, Is.Not.Empty);
        }

        [TestCase("admin@hotel.com", true)]
        [TestCase("ADMIN@hotel.com", true)]
        [TestCase("xxxxx@hotel.com", false)]
        public void GetSingleUser_WhenCalled_ReturnsUserOrNull(string emailStr, bool expectedResult)
        {
            var result = _service.GetSingleUser(emailStr).GetAwaiter().GetResult();

            Assert.That(result?.Email != null, Is.EqualTo(expectedResult));
        }

        [TestCase("admin@hotel.com", true)]
        [TestCase("ADMIN@hotel.com", true)]
        [TestCase("xxxxx@hotel.com", false)]
        public void IsEmailExisted_WhenCalled_ReturnsUserOrNull(string emailStr, bool expectedResult)
        {
            var result = _service.IsEmailExisted(emailStr).GetAwaiter().GetResult();

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(1, true)]
        [TestCase(3, true)]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        public void GetSingleUser_WhenCalled_ReturnsUserOrNull(int id, bool expectedResult)
        {
            var result = _service.GetSingleUser(id).GetAwaiter().GetResult();

            Assert.That(result?.Id != null, Is.EqualTo(expectedResult));
        }

        [TestCase("existed@email.com")]
        public void AddUser_WhenNameIsExisted_ReturnNull(string emailStr)
        {
            // Arrange
            // Act
            var result = _service.AddOrUpdateUser(new User { Email = emailStr }).GetAwaiter().GetResult();
            var repeat = _service.AddOrUpdateUser(new User { Email = emailStr }).GetAwaiter().GetResult();

            // Assert
            System.Console.WriteLine($"{result.Id} : {result.Email }");
            Assert.That(repeat, Is.Null);
        }

        [TestCase("unique@email.com")]
        [TestCase("uniquee@email.com")]
        [TestCase("uniqueee@email.com")]
        public void AddUser_WhenNameIsUnique_ReturnAddedUser(string emailstr)
        {
            // Act
            var result = _service.AddOrUpdateUser(new User { Email = emailstr }).GetAwaiter().GetResult();

            // Assert
            System.Console.WriteLine($"{result.Id} : {result.Email }");
            Assert.That(result.Id >= 1);
            Assert.That(result.Email.Equals(emailstr));
        }

        // 1 no user
        [TestCase("211", "marketing", "receptionist")]
        public void UpdateUserRoles_WhenNoExistedUser_ReturnsNull(int NoExistedUserId, string roleName01, string roleName02)
        {
            // Arrange
            var role1 = _service.GetSingleRole(roleName01).GetAwaiter().GetResult();
            var role2 = _service.GetSingleRole(roleName02).GetAwaiter().GetResult();
            var role1Id = role1 == null ? 0 : role1.Id;
            var role2Id = role2 == null ? 0 : role2.Id;
            var roleIds = new List<int> { role1Id, role2Id };

            // Act
            var result = _service.UpdateUserRoles(NoExistedUserId, roleIds).GetAwaiter().GetResult();

            // Assert
            Utilities.PrintOut(result);
            Assert.That(result, Is.Null);
        }

        // 2 no userroles
        [TestCase("marketing", "receptionist", 2)]
        public void UpdateUserRoles_WhenRoleIdsIsNull_ReturnsOldUserRoles(string roleName01, string roleName02, int expectedResult)
        {
            // Arrange
            var user = _service.GetSingleUser("nn@hotel.com").GetAwaiter().GetResult();
            var role1 = _service.GetSingleRole(roleName01).GetAwaiter().GetResult();
            var role2 = _service.GetSingleRole(roleName02).GetAwaiter().GetResult();
            var userId = user == null ? 0 : user.Id;
            var role1Id = role1 == null ? 0 : role1.Id;
            var role2Id = role2 == null ? 0 : role2.Id;
            var roleIds = new List<int> { role1Id, role2Id };

            // Act  initial userroles
            var result = _service.UpdateUserRoles(user.Id, roleIds).GetAwaiter().GetResult();

            // Assert
            Utilities.PrintOut(result);
            Assert.That(result.Count == expectedResult);

            // Act Again
            var secondResult = _service.UpdateUserRoles(user.Id, null).GetAwaiter().GetResult();

            // Assert  show the older userroles
            Utilities.PrintOut(secondResult);
            Assert.That(result.Count == expectedResult);
            Assert.That(secondResult.All(result.Contains) && secondResult.Count == result.Count);
        }

        // 3 userroles has wrong id
        [TestCase(981)]
        public void UpdateUserRoles_WhenHasNoExistedRole_ReturnsNewUserRoles(int NoExistedRoleId)
        {
            // Arrange
            var user = _service.GetSingleUser("nn@hotel.com").GetAwaiter().GetResult();
            var role1 = _service.GetSingleRole("marketing").GetAwaiter().GetResult();
            var role2 = _service.GetSingleRole("receptionist").GetAwaiter().GetResult();
            var roleIds = new List<int> { role1.Id, role2.Id, NoExistedRoleId };

            // Act
            var result = _service.UpdateUserRoles(user.Id, roleIds).GetAwaiter().GetResult();

            // Assert
            Utilities.PrintOut(result);
            Assert.That(result.Count == 2);
        }

        // 4 normally
        [TestCase("marketing", "receptionist", 2)]
        [TestCase("mark", "receptionist", 1)]
        [TestCase("marketing", "recept", 1)]
        [TestCase("market", "recept", 0)]
        [TestCase("market", "", 0)]
        [TestCase("", "", 0)]
        public void UpdateUserRoles_WhenCalled_ReturnNewUserRoles(string roleName01, string roleName02, int expectedResult)
        {
            // Arrange
            var user = _service.GetSingleUser("nn@hotel.com").GetAwaiter().GetResult();
            var role1 = _service.GetSingleRole(roleName01).GetAwaiter().GetResult();
            var role2 = _service.GetSingleRole(roleName02).GetAwaiter().GetResult();
            var userId = user == null ? 0 : user.Id;
            var role1Id = role1 == null ? 0 : role1.Id;
            var role2Id = role2 == null ? 0 : role2.Id;
            var roleIds = new List<int> { role1Id, role2Id };

            // Act
            var result = _service.UpdateUserRoles(userId, roleIds).GetAwaiter().GetResult();

            // Assert
            Utilities.PrintOut(result);
            Assert.That(result.Count == expectedResult);
        }



        private void PrintOutUsers(IQueryable<User> users)
        {
            var userInfo = users.Select(r => new
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
            Utilities.PrintOut(userInfo);
        }
    }
}
