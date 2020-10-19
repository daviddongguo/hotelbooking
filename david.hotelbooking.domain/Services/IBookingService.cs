using david.hotelbooking.domain.Entities.Hotel;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.domain.Services
{
    public interface IBookingService
    {
        Task<Booking> AddBooking(Booking toAddBooking);
        Task DeleteBooking(int id);
        Task<Guest> AddGuest(Guest toAddGuest);
        Task<bool> DoesOverlap(Booking booking);
        Task<IQueryable<Booking>> GetAllBookings();
        Task<IQueryable<Room>> GetAllRooms();
        Task<Booking> GetBookingById(int bookId);
        Task<IQueryable<Booking>> GetBookings(int roomId = 0, string guestEmailOrName = null);
        Task<Booking> GetBookingsByEmail(string email);
        Task<IQueryable<Booking>> GetBookingsByGuestName(string name);
        Task<Booking> GetBookingsById(int bookingId);
        Task<Guest> GetGuestByEmail(string email);
        Task<Guest> GetGuestById(int id);
        Task<IQueryable<Guest>> GetGuestByName(string name);
        Task<Room> GetRoomById(int roomId);
        Task<bool> IsEmailExisted(string email);
        Task<Booking> SearchOverlappingBooking(Booking booking);
        Task<Booking> SearchOverlappingBooking(Booking booking, IQueryable<Booking> bookings);
        Task<Booking> UpdateBookingDate(Booking toUpdateBooking);
        Task<Booking> UpdateBookingRoom(Booking toUpdateBooking);
    }
}
