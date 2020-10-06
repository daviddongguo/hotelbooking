﻿using david.hotelbooking.domain.Abstract;
using david.hotelbooking.domain.Entities.RBAC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace david.hotelbooking.domain.Concretes
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
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