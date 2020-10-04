using david.hotelbooking.domain.Abstract;
using david.hotelbooking.domain.Entities.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace david.hotelbooking.domain.Concretes
{
    public class EFUserRepository : IUserRepository
    {
        private readonly EFDbContext _context;

        public EFUserRepository(EFDbContext context)
        {
            _context = context;
        }

       public IQueryable<User> GetAll()
        {
            return _context.Users.AsQueryable();
        }
    }
}
