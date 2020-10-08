using System;
using System.Collections.Generic;
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
        public async Task<User> GetSingleUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
               id == u.Id
            );
        }

        public async Task<User> AddUser(User inputUser)
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
