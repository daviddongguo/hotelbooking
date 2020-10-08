using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.UnitTests;
using david.hotelbooking.UnitTests.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public void GetSingleUser_WhenCalled_RetunsUserOrNull(string emailStr, bool expectedResult)
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

        // user: "nn@hotl.com" add "marketing" adn "receptionist" roles
        [TestCase("marketing", "receptionist", true)]
        [TestCase("mark", "receptionist", false)]
        [TestCase("marketing", "recept", false)]
        [TestCase("market", "recept", false)]
        public void UpdateUserRoles_WhenCalled_ReturnNewUserRoles(string roleName01, string roleName02, bool expectedResult)
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
            var result = _service.UpdateUserRoles(user.Id, roleIds).GetAwaiter().GetResult();

            // Assert
            PrintOut(result);
            Assert.That(result.Count == 2, Is.EqualTo(expectedResult));
        }


        [TestCase("981")]
        public void UpdateUserRoles_WhenUserNoExistedUser_ReturnNull(int NoExistedUserId)
        {
            // Arrange
            var role1 = _service.GetSingleRole("marketing").GetAwaiter().GetResult();
            var role2 = _service.GetSingleRole("receptionist").GetAwaiter().GetResult();
            var roleIds = new List<int> { role1.Id, role2.Id };

            // Act
            var result = _service.UpdateUserRoles(NoExistedUserId, roleIds).GetAwaiter().GetResult();

            // Assert
            PrintOut(result);
            Assert.That(result, Is.Null);
        }


        [Test]
        public void UpdateUserRoles_WhenAllRolesNotExisted_ReturnNull()
        {
            // Arrange
            var user = _service.GetSingleUser("nn@hotel.com").GetAwaiter().GetResult();
            var roleIds = new List<int>();

            // Act
            var result = _service.UpdateUserRoles(user.Id, roleIds).GetAwaiter().GetResult();

            // Assert
            PrintOut(result);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateUserRoles_WhenNoRoles_ReturnNull()
        {
            // Arrange
            var user = _service.GetSingleUser("nn@hotel.com").GetAwaiter().GetResult();
            var roleIds = new List<int>();

            // Act
            var result = _service.UpdateUserRoles(user.Id, roleIds).GetAwaiter().GetResult();

            // Assert
            PrintOut(result);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateUserRoles_WhenRolesIsNull_ReturnNull()
        {
            // Arrange
            var user = _service.GetSingleUser("nn@hotel.com").GetAwaiter().GetResult();

            // Act
            var result = _service.UpdateUserRoles(user.Id, null).GetAwaiter().GetResult();

            // Assert
            PrintOut(result);
            Assert.That(result, Is.Null);
        }

        [TestCase(981)]
        public void UpdateUserRoles_WhenUserNoExistedRole_ReturnsUserRoles(int NoExistedRoleId)
        {
            // Arrange
            var user = _service.GetSingleUser("nn@hotel.com").GetAwaiter().GetResult();
            var role1 = _service.GetSingleRole("marketing").GetAwaiter().GetResult();
            var role2 = _service.GetSingleRole("receptionist").GetAwaiter().GetResult();
            var roleIds = new List<int> { role1.Id, role2.Id, NoExistedRoleId };

            // Act
            var result = _service.UpdateUserRoles(user.Id, roleIds).GetAwaiter().GetResult();

            // Assert
            // Assert
            PrintOut(result);
            Assert.That(result.Count == 2);
        }

        private void PrintOut(Object obj)
        {
            System.Console.WriteLine(Utilities.PrettyJson(JsonSerializer.Serialize(obj)));

        }


    }
}
