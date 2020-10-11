using david.hotelbooking.domain.Abstract;
using david.hotelbooking.domain.Entities.RBAC;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace david.hotelbooking.domain.Concretes
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public IQueryable<UserRole> GetAll()
        {
            return _context.UserRoles
                .Include(u => u.User)
                .Include(u => u.Role).AsQueryable();
        }
        public IQueryable<User> GetAllUsers()
        {
            return _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(uu => uu.Role)
            .AsQueryable();
        }


    }
}
