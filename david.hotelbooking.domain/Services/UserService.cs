using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.RBAC;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.domain.Services
{
    public class UserService : IUserService
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
            .ThenInclude(r => r.RolePermissions)
            .ThenInclude(rr => rr.Permission)
            .ToListAsync())
            .AsQueryable();
        }


        public async Task<User> GetSingleUser(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
               email.ToLower().Equals(u.Email.ToLower())
            );
        }
        public async Task<bool> IsEmailExisted(string email)
        {
            var dbUser = await GetSingleUser(email);
            return dbUser != null;      // returns true when user exists
        }
        public async Task<Role> GetSingleRole(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(u =>
               roleName.ToLower().Equals(u.Name.ToLower())
            );
        }
        public async Task<User> GetSingleUser(int? id)
        {
            return await _context.Users
                .Include( u => u.UserRoles)
                .ThenInclude( uu => uu.Role)
                .FirstOrDefaultAsync(u =>
               id == u.Id
            );
        }

        public async Task<User> AddOrUpdateUser(User inputUser)
        {
            if (inputUser == null)
            {
                return null;
            }

            // Add When User's Id is null or 0
            if (inputUser?.Id == null || inputUser.Id == 0)
            {
                if (IsEmailExisted(inputUser.Email).GetAwaiter().GetResult())
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
            else // Update when User's Id exists
            {
                var toUpdateDbUser = GetSingleUser(inputUser.Id).GetAwaiter().GetResult();
                if (toUpdateDbUser == null)
                {
                    return null;
                }

                toUpdateDbUser.Password = inputUser.Password;

                await _context.SaveChangesAsync();
                return toUpdateDbUser;
            }
        }

        public async Task<bool> DeleteUser(int? id)
        {
            var toDeleteDbUser = await GetSingleUser(id);
            if (toDeleteDbUser == null)
            {
                return false;
            }
            try
            {
                _context.Users.Remove(toDeleteDbUser);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception e)
            {
                throw e;
            }

        }

        public async Task<List<UserRole>> UpdateUserRoles(int toUpdateUserId, List<int> toAddOrUpdateRoleIds)
        {
            var dbUser = await GetSingleUser(toUpdateUserId);
            if (dbUser == null)
            {
                return null;
            }

            var oldUserRolesList = _context.UserRoles.Where(u => u.UserId == dbUser.Id).ToListAsync().GetAwaiter().GetResult();
            if (toAddOrUpdateRoleIds == null) // do nothing, just show existed info.
            {
                return oldUserRolesList;
            }

            var newUsersRoleList = new List<UserRole>();
            foreach (var roleId in toAddOrUpdateRoleIds)
            {
                var dbRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                if (dbRole == null)
                {
                    continue;
                }

                var newUserRole = new UserRole
                {
                    UserId = dbUser.Id,
                    RoleId = dbRole.Id
                };
                newUsersRoleList.Add(newUserRole);
            }


            _context.UserRoles.RemoveRange(oldUserRolesList);
            _context.UserRoles.AddRange(newUsersRoleList);
            await _context.SaveChangesAsync();

            return newUsersRoleList;

            // var toRemoveList = _context.UserRoles.Where(u => u.UserId == dbUser.Id);
            // using (var transaction = _context.Database.BeginTransaction())
            // {
            //     try
            //     {
            //         _context.UserRoles.RemoveRange(toRemoveList);
            //         await _context.SaveChangesAsync();
            //         _context.UserRoles.AddRange(resultList);
            //         await _context.SaveChangesAsync();

            //         transaction.Commit();
            //     }
            //     catch (Exception)
            //     {
            //         return null;
            //     }
            // };
        }
    }




}
