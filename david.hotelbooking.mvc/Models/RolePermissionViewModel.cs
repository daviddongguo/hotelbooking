using david.hotelbooking.domain.Entities.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.mvc.Models
{
    public class RolePermissionViewModel
    {
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public List<int> RolePermissionIds { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
