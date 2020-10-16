using david.hotelbooking.domain.Entities.Hotel;
using Microsoft.EntityFrameworkCore;

namespace david.hotelbooking.domain.Concretes
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext() : base()
        {

        }
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        public virtual DbSet<RoomGroup> RoomGroups { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // composite key
            modelBuilder.Entity<Booking>()
                .HasKey(u => new { u.GuestId, u.RoomId });

            // Seeding
            modelBuilder.Entity<RoomGroup>().HasData(
                new RoomGroup { Id = 1, Name = "Gold Building" },
                new RoomGroup { Id = 2, Name = "Silver Building" }
                );
            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { Id = 1, Name = "Standard" },
                new RoomType { Id = 2, Name = "Luxury" });

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomGroupId = 1, RoomTypeId = 1, RoomNumber = "801" },
                new Room { Id = 2, RoomGroupId = 1, RoomTypeId = 2, RoomNumber = "802" },
                new Room { Id = 3, RoomGroupId = 1, RoomTypeId = 1, RoomNumber = "803" },
                new Room { Id = 4, RoomGroupId = 1, RoomTypeId = 2, RoomNumber = "804" },
                new Room { Id = 5, RoomGroupId = 2, RoomTypeId = 1, RoomNumber = "901" },
                new Room { Id = 6, RoomGroupId = 2, RoomTypeId = 2, RoomNumber = "902" },
                new Room { Id = 7, RoomGroupId = 2, RoomTypeId = 1, RoomNumber = "903" },
                new Room { Id = 8, RoomGroupId = 2, RoomTypeId = 2, RoomNumber = "904" }
                );

        }


    }
}
