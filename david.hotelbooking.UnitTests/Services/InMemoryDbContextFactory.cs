using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.RBAC;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace david.hotelbooking.UnitTests.Services
{
    public class InMemoryDbContextFactory
    {
        public UserDbContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                        .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                        .Options;
            var dbContext = new UserDbContext(options);

            Seed(dbContext);

            return dbContext;

        }

        private void Seed(UserDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();


            var userAdmin = new User { Email = "Admin@hotel.com", Password = "aaa" };
            var userAdminn = new User { Email = "nn@hotel.com", Password = "aaa" };
            var userAdminnn = new User { Email = "nnn@hotel.com", Password = "aaa" };
            var userSis = new User { Email = "Sis@s.s", Password = "aaa" };
            dbContext.Users.Add(userAdmin);
            dbContext.Users.Add(userAdminn);
            dbContext.Users.Add(userAdminnn);
            dbContext.Users.Add(userSis);

            var roleAdmin = new Role { Name = "admin" };
            var roleMarketing = new Role { Name = "marketing" };
            var roleReceptionist = new Role { Name = "receptionist" };
            var roleCustomer = new Role { Name = "customer" };
            dbContext.Roles.Add(roleAdmin);
            dbContext.Roles.Add(roleMarketing);
            dbContext.Roles.Add(roleReceptionist);
            dbContext.Roles.Add(roleCustomer);

            var permissionReadOrder = new Permission { Name = "readOrder" };
            var permissionWriteOrder = new Permission { Name = "writeOrder" };
            var permissionReadUser = new Permission { Name = "readUser" };
            var permissionWriteUser = new Permission { Name = "writeUser" };
            dbContext.Permissions.Add(permissionReadOrder);
            dbContext.Permissions.Add(permissionWriteOrder);
            dbContext.Permissions.Add(permissionReadUser);
            dbContext.Permissions.Add(permissionWriteUser);

            dbContext.SaveChangesAsync().GetAwaiter();

            dbContext.UserRoles.Add(new UserRole { UserId = userAdmin.Id, RoleId = roleAdmin.Id });
            dbContext.UserRoles.Add(new UserRole { UserId = userSis.Id, RoleId = roleCustomer.Id });

            dbContext.RolePermissions.Add(new RolePermission { RoleId = roleAdmin.Id, PermissionId = permissionReadOrder.Id });
            dbContext.RolePermissions.Add(new RolePermission { RoleId = roleAdmin.Id, PermissionId = permissionWriteOrder.Id });
            dbContext.RolePermissions.Add(new RolePermission { RoleId = roleAdmin.Id, PermissionId = permissionReadUser.Id });
            dbContext.RolePermissions.Add(new RolePermission { RoleId = roleAdmin.Id, PermissionId = permissionWriteUser.Id });

            dbContext.RolePermissions.Add(new RolePermission { RoleId = roleCustomer.Id, PermissionId = permissionReadOrder.Id });
            dbContext.RolePermissions.Add(new RolePermission { RoleId = roleCustomer.Id, PermissionId = permissionReadUser.Id });


            dbContext.SaveChangesAsync().GetAwaiter();
        }

    }
}
