using david.hotelbooking.domain.Entities.RBAC;
using System.Collections.Generic;
using System.Linq;

namespace david.hotelbooking.mvc.Models
{
    public class UserRolesViewModel
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public List<int> UserRoleIds { get; set; }
        public List<Role> Roles { get; set; }
    }
}
