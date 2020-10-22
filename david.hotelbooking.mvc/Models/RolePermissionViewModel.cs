using david.hotelbooking.domain.Entities.RBAC;
using System.Collections.Generic;

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
