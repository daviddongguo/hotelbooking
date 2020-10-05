using david.hotelbooking.domain.Entities.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace david.hotelbooking.domain.Abstract
{
    public interface IUserRepository
    {
        IQueryable<UserRole> GetAll();
    }
}
