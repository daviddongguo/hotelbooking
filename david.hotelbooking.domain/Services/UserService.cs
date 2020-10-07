using System.Linq;
using System.Threading.Tasks;
using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.RBAC;
using Microsoft.EntityFrameworkCore;

namespace david.hotelbooking.domain.Services
{
    public class UserService
    {
        private UserDbContext _context;

        public UserService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<User>> GetAllUsers()
        {
            return (await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(uu => uu.Role)
            .ToListAsync())
            .AsQueryable();
        }

        public async Task<User> GetSingleUser(string email = "", int id = 0)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
               id == u.Id || email == u.Email
            );
        }

        public async Task<User> AddUser(User inputUser)
        {
            var dbUser = await GetSingleUser(inputUser.Email);
            if (dbUser != null)
            {
                return null;
            }

            var newUser = new User
            {
                Email = inputUser.Email,
                Password = inputUser.Password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
        public async Task<UserRole> AddRole(User toUpdateUser, Role toAddRole)
        {
            var dbUser = await GetSingleUser("", toUpdateUser.Id);
            var dbRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == toAddRole.Id);
            var dbUserRole = await _context.UserRoles.FirstOrDefaultAsync( 
                r => r.UserId == toUpdateUser.Id && r.RoleId == toAddRole.Id);
            if (dbUser == null || dbRole == null || dbUserRole != null)
            {
                return null;
            }

            var newUserRole = new UserRole
            {
                UserId = dbUser.Id,
                RoleId = dbRole.Id
            };
            _context.UserRoles.Add(newUserRole);
            await _context.SaveChangesAsync();
            return newUserRole;
        }
    }




}
