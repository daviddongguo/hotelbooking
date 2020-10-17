using david.hotelbooking.domain.Entities.Hotel;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.domain.Services
{
    public interface IBookingService
    {
        Task<Guest> AddGuest(Guest toAddGuest);
        Task<IQueryable<Booking>> GetAllBookings(int roomId = 0, string guestEmailOrName = null);
        Task<IQueryable<Room>> GetAllRooms();
        Task<IQueryable<Guest>> GetGuestByEmail(string email);
        Task<Guest> GetGuestById(int id);
        Task<IQueryable<Guest>> GetGuestByName(string name);
        Task<bool> IsEmailExisted(string email);
    }
}