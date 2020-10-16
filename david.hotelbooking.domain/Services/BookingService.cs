using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.Hotel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .ToListAsync()).AsQueryable();
        }
    }
}
