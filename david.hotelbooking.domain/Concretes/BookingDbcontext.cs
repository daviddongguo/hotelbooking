using david.hotelbooking.domain.Entities.Hotel;
using Microsoft.EntityFrameworkCore;
using System;

namespace david.hotelbooking.domain.Concretes
{
    public class BookingDbContext : DbContext
    {
        private readonly DateTime firstDay = DateTime.Now.Date + new TimeSpan(14, 0, 0);
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

            modelBuilder.Entity<Guest>().HasData(
                new Guest { Id = 1, Name = "Ava", Email = "ava@ho.t" },
                new Guest { Id = 2, Name = "Amelia", Email = "amelia@ho.t" },
                new Guest { Id = 3, Name = "Aiden", Email = "aiden@ho.t" },
                new Guest { Id = 4, Name = "Austin", Email = "austin@ho.t" },
                new Guest { Id = 5, Name = "Aaron", Email = "aaron@ho.t" },
                new Guest { Id = 6, Name = "Axel", Email = "axel@ho.t" },
                new Guest { Id = 7, Name = "Adam", Email = "adam@ho.t" },
                new Guest { Id = 8, Name = "Alice", Email = "alice@ho.t" }
                );

            modelBuilder.Entity<Booking>().HasData(
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
                );

        }

        internal void changeSaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
