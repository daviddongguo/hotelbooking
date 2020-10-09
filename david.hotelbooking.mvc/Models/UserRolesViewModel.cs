using david.hotelbooking.domain.Entities.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.mvc.Models
{
    public class UserRolesViewModel
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
    }
}
