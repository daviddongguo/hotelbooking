using david.hotelbooking.domain.Entities.Hotel;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.domain.Services
{
    public interface IBookingService
    {
        Task<IQueryable<Room>> GetAllRooms();
    }
}