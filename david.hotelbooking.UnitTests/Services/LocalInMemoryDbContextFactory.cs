using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Entities.RBAC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace david.hotelbooking.UnitTests.Services
{
    public class LocalInMemoryDbContextFactory
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


            var userAdmin = new RBAC_User { Email = "Admin@hotel.com", Password = "aaa" };
            var userAdminn = new RBAC_User { Email = "nn@hotel.com", Password = "aaa" };
            var userAdminnn = new RBAC_User { Email = "nnn@hotel.com", Password = "aaa" };
            var userSis = new RBAC_User { Email = "Sis@s.s", Password = "aaa" };
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


        public BookingDbContext GetBookingContext()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                        .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                        .Options;
            var dbContext = new BookingDbContext(options);

            Seed(dbContext);

            return dbContext;

        }

        private void Seed(BookingDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            dbContext.RoomGroups.AddRange(new List<RoomGroup>
            {
                new RoomGroup { Id = 1, Name = "Gold Building" },
                new RoomGroup { Id = 2, Name = "Silver Building" }
            });

            dbContext.RoomTypes.AddRange(new List<RoomType>
            {
                new RoomType { Id = 1, Name = "Standard" },
                new RoomType { Id = 2, Name = "Luxury" }
            });
            dbContext.Rooms.AddRange(new List<Room>
            {
                new Room { Id = 1, RoomGroupId = 1, RoomTypeId = 1, RoomNumber = "801" },
                new Room { Id = 2, RoomGroupId = 1, RoomTypeId = 2, RoomNumber = "802" },
                new Room { Id = 3, RoomGroupId = 1, RoomTypeId = 1, RoomNumber = "803" },
                new Room { Id = 4, RoomGroupId = 1, RoomTypeId = 2, RoomNumber = "804" },
                new Room { Id = 5, RoomGroupId = 2, RoomTypeId = 1, RoomNumber = "901" },
                new Room { Id = 6, RoomGroupId = 2, RoomTypeId = 2, RoomNumber = "902" },
                new Room { Id = 7, RoomGroupId = 2, RoomTypeId = 1, RoomNumber = "903" },
                new Room { Id = 8, RoomGroupId = 2, RoomTypeId = 2, RoomNumber = "904" }

            });

            dbContext.Guests.AddRange(new List<Guest> {
                new Guest { Id = 1, Name = "Ava", Email = "ava@ho.t" },
                new Guest { Id = 2, Name = "Amelia", Email = "amelia@ho.t" },
                new Guest { Id = 3, Name = "Aiden", Email = "aiden@ho.t" },
                new Guest { Id = 4, Name = "Austin", Email = "austin@ho.t" },
                new Guest { Id = 5, Name = "Aaron", Email = "aaron@ho.t" },
                new Guest { Id = 6, Name = "Axel", Email = "axel@ho.t" },
                new Guest { Id = 7, Name = "Adam", Email = "adam@ho.t" },
                new Guest { Id = 8, Name = "Alice", Email = "alice@ho.t" }
            });

            DateTime firstDay = DateTime.Now.Date + new TimeSpan(14, 0, 0);

            dbContext.Bookings.AddRange(new List<Booking>
            {
                new Booking { Id = 1, GuestId = 1, RoomId = 1, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(1).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 2, GuestId = 2, RoomId = 2, FromDate = firstDay.AddDays(1), ToDate = firstDay.AddDays(3).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 3, GuestId = 3, RoomId = 3, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(3).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 4, GuestId = 4, RoomId = 4, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(4).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 5, GuestId = 5, RoomId = 5, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(5).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 6, GuestId = 6, RoomId = 6, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(6).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 7, GuestId = 7, RoomId = 7, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(7).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 8, GuestId = 8, RoomId = 8, FromDate = firstDay.AddDays(0), ToDate = firstDay.AddDays(8).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 9, GuestId = 8, RoomId = 1, FromDate = firstDay.AddDays(1), ToDate = firstDay.AddDays(8).Date + new TimeSpan(10, 0, 0) },
                new Booking { Id = 10, GuestId = 8, RoomId = 2, FromDate = firstDay.AddDays(4), ToDate = firstDay.AddDays(8).Date + new TimeSpan(10, 0, 0) }
            });

            dbContext.SaveChangesAsync().GetAwaiter();
        }


    }
}
