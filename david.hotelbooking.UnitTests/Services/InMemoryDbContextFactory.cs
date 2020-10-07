using david.hotelbooking.domain.Concretes;
using Microsoft.EntityFrameworkCore;

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

            return dbContext;

        }
    }
}
