using david.hotelbooking.domain.Concretes;
using Microsoft.EntityFrameworkCore;

namespace david.hotelbooking.UnitTests.Services
{
    public class RemoteMySqlDbcontextFactory
    {


        public BookingDbContext GetBookingContext()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                        .UseMySql(ConfigurationManager.AppSetting["ConnectionStrings:MySqlConnection"],
                        op => op.EnableRetryOnFailure())
                        .Options;
            var dbContext = new BookingDbContext(options);

            return dbContext;

        }
        public UserDbContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                        .UseMySql(ConfigurationManager.AppSetting["ConnectionStrings:MySqlConnection"],
                        op => op.EnableRetryOnFailure())
                        .Options;
            var dbContext = new UserDbContext(options);

            return dbContext;

        }

    }
}
