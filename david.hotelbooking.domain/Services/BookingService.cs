using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.Hotel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.domain.Services
{
    public class BookingService :  IBookingService
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

        public async Task<IQueryable<Booking>> GetAllBookings(int roomId = 0, string guestEmailOrName = null)
        {
            return (await _context.Bookings
                .Where(r => roomId == 0 || r.RoomId == roomId)
                .Where(r => guestEmailOrName == null || r.Guest.Email.ToLower() == guestEmailOrName.ToLower() || r.Guest.Name.ToLower() == guestEmailOrName.ToLower())
                .Include(b => b.Guest)
                .ToListAsync())
                .AsQueryable();
        }

        public async Task<IQueryable<Guest>> GetGuestByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            return (await _context.Guests.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToListAsync()).AsQueryable();
        }
        public async Task<IQueryable<Guest>> GetGuestByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            return (await _context.Guests.Where(g => g.Email.ToLower().Contains(email.ToLower())).ToListAsync()).AsQueryable();
        }

        public async Task<Guest> GetGuestById(int id)
        {
            return await _context.Guests.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> IsEmailExisted(string email)
        {
            return (await GetGuestByEmail(email))?.Count() >= 1;
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




    }
}
