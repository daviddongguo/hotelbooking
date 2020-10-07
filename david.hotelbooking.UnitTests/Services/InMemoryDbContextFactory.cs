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
            dbContext.Database.EnsureCreated();


            dbContext.Users.Add(new User { Email = "Admin@hotel.com", Password = "aaa" });
            dbContext.Users.Add(new User { Email = "Adminn@hotel.com", Password = "aaa" });
            dbContext.Users.Add(new User { Email = "Adminnn@hotel.com", Password = "aaa" });
            dbContext.SaveChanges();

        }

    }
}
