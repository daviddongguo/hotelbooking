using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.Hotel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.domain.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingDbContext _context;
        public BookingService(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Room>> GetAllRooms()
        {
            return (await _context.Rooms
                .Include(r => r.RoomGroup)
                .Include(r => r.RoomType)
                .ToListAsync())
                .AsQueryable();
        }

        public async Task<Booking> GetBookingsByEmail(string email)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.Guest.Email == email);
        }

        public async Task<Booking> GetBookingsById(int bookingId)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<IQueryable<Booking>> GetAllBookings()
        {
            var result = await _context.Bookings
            .Include(b => b.Guest)
            .Include(b => b.Room)
            .ToListAsync();

            return result.AsQueryable();
        }

        public async Task<IQueryable<Booking>> GetBookingsByGuestName(string name)
        {
            return await GetBookings(0, name);
        }

        public async Task<IQueryable<Booking>> GetBookings(int roomId = 0, string guestEmailOrName = null)
        {
            // ToDo: Unknown column 'g.Email' in 'field list'",
            return (await _context.Bookings
                .Where(r => roomId == 0 || r.RoomId == roomId)
                .Where(r => guestEmailOrName == null || r.Guest.Email.ToLower() == guestEmailOrName.ToLower() || r.Guest.Name.ToLower() == guestEmailOrName.ToLower())
                .Include(b => b.Guest)
                .ToListAsync())
                .AsQueryable();
        }
        public async Task<Booking> SearchOverlappingBooking(Booking booking, IQueryable<Booking> bookings)
        {
            if (booking == null || bookings == null)
            {
                return null;
            }
            var overlappingBooking = await bookings
                    .FirstOrDefaultAsync
                    (b => b.RoomId == booking.RoomId &&
                        booking.FromDate < b.ToDate && booking.ToDate > b.FromDate
                    );

            return overlappingBooking;
        }

        public async Task<Booking> SearchOverlappingBooking(Booking booking)
        {
            return await SearchOverlappingBooking(booking, _context.Bookings);
        }

        public async Task<bool> DoesOverlap(Booking booking)
        {
            // true : overlop : overloppingbooking existed
            return await SearchOverlappingBooking(booking) != null;
        }


        public async Task<IQueryable<Guest>> GetGuestByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            return (await _context.Guests.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToListAsync()).AsQueryable();
        }
        public async Task<Guest> GetGuestByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            return (await _context.Guests.FirstOrDefaultAsync(g => g.Email.ToLower().Equals(email.ToLower())));
        }

        public async Task<Guest> GetGuestById(int id)
        {
            return await _context.Guests.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsEmailExisted(string email)
        {
            return (await GetGuestByEmail(email)) != null;
        }

        public async Task<Guest> AddGuest(Guest toAddGuest)
        {
            if (string.IsNullOrWhiteSpace(toAddGuest.Email) || await IsEmailExisted(toAddGuest.Email))
            {
                return null;
            }
            try
            {
                var newGuest = new Guest
                {
                    Name = toAddGuest.Name,
                    Email = toAddGuest.Email
                };
                await _context.Guests.AddAsync(newGuest);
                await _context.SaveChangesAsync();
                return newGuest;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteBooking(int? bookId)
        {
            if (bookId == null)
            {
                return;
            }
            var dbBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookId);
            if (dbBooking == null)
            {
                return;
            }
            try
            {
                _context.Bookings.Remove(dbBooking);
                await _context.SaveChangesAsync();
                return;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public async Task<Booking> AddBooking(Booking toAddBooking)
        {
            if (toAddBooking == null)
            {
                return null;
            }
            var roomDb = await GetRoomById(toAddBooking.RoomId);
            var guestDb = await GetGuestById(toAddBooking.GuestId);
            if (roomDb == null || guestDb == null || await DoesOverlap(toAddBooking))
            {
                return null;
            }
            if (toAddBooking.FromDate >= toAddBooking.ToDate)
            {
                return null;
            }
            try
            {
                //toAddBooking.Id = 0;
                //toAddBooking.Room = roomDb;
                //toAddBooking.Guest = guestDb;
                await _context.Bookings.AddAsync(toAddBooking);
                await _context.SaveChangesAsync();
                return toAddBooking;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task<Booking> UpdateBookingDate(Booking toUpdateBooking)
        {
            if (toUpdateBooking == null)
            {
                return null;
            }
            if (toUpdateBooking.FromDate >= toUpdateBooking.ToDate)
            {
                return null;
            }

            var bookingDb = await GetBookingById(toUpdateBooking.Id);
            if (bookingDb == null)
            {
                return null;
            }

            try
            {
                bookingDb.FromDate = toUpdateBooking.FromDate;
                bookingDb.ToDate = toUpdateBooking.ToDate;
                await _context.SaveChangesAsync();
                return bookingDb;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task<Booking> UpdateBookingRoom(Booking toUpdateBooking)
        {
            // Verify input value
            if (toUpdateBooking == null)
            {
                return null;
            }

            var oldDbBooking = await GetBookingById(toUpdateBooking.Id);
            var newDbRoom = await GetRoomById(toUpdateBooking.RoomId);
            if (oldDbBooking == null || newDbRoom == null)
            {
                return null;
            }

            var newBooking = new Booking();
            try
            {
                newBooking.RoomId = newDbRoom.Id;
                newBooking.GuestId = oldDbBooking.GuestId;
                newBooking.FromDate = oldDbBooking.FromDate;
                newBooking.ToDate = oldDbBooking.ToDate;

                _context.Bookings.Remove(oldDbBooking);
                _context.Bookings.Add(newBooking);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                throw e;
            }

            return newBooking;
        }

        public async Task<Booking> GetBookingById(int bookId)
        {
            return await _context.Bookings.Include(r => r.Room).FirstOrDefaultAsync(r => r.Id == bookId); ;
        }

        public async Task<Room> GetRoomById(int roomId)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<Guest> AddGuest(string email, string name = "")
        {
            if (email == null || await IsEmailExisted(email))
            {
                return null;
            }
            try
            {
                Guest guest = new Guest{
                    Email = email,
                    Name = name,
                };
                await _context.AddAsync(guest);
                await _context.SaveChangesAsync();
                return guest;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
