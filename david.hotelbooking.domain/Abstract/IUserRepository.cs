using david.hotelbooking.domain.Entities.RBAC;
using System.Linq;

namespace david.hotelbooking.domain.Abstract
{
    public interface IUserRepository
    {
        IQueryable<UserRole> GetAll();
        IQueryable<User> GetAllUsers();
    }
}
