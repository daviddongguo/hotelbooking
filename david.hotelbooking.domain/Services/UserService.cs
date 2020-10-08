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

        public async Task<Role> GetSingleRole(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(u =>
               roleName.ToLower().Equals(u.Name.ToLower())
            );
        }
        public async Task<User> GetSingleUser(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
               email.ToLower().Equals(u.Email.ToLower())
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
        public async Task<List<UserRole>> UpdateUserRoles(int toUpdateUserId, List<int> toAddOrUpdateRoleIds)
        {
            var dbUser = await GetSingleUser(toUpdateUserId);
            if (dbUser == null || toAddOrUpdateRoleIds == null || toAddOrUpdateRoleIds.Count() == 0)
            {
                return null;
            }

            var resultList = new List<UserRole>();
            foreach (var roleId in toAddOrUpdateRoleIds)
            {
                var dbRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                if (dbRole == null)
                {
                    continue;
                }

                var dbUserRole = await _context.UserRoles.FirstOrDefaultAsync(
                    r => r.UserId == dbUser.Id && r.RoleId == dbRole.Id);
                if (dbUserRole != null)
                {
                    continue;
                }
                var newUserRole = new UserRole
                {
                    UserId = dbUser.Id,
                    RoleId = dbRole.Id
                };
                resultList.Add(newUserRole);
            }

            var toRemoveList = _context.UserRoles.Where(u => u.UserId == dbUser.Id);

            _context.UserRoles.RemoveRange(toRemoveList);
            await _context.SaveChangesAsync();
            _context.UserRoles.AddRange(resultList);
            await _context.SaveChangesAsync();


            return resultList;

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
