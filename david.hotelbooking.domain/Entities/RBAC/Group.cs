using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace david.hotelbooking.domain.Entities.RBAC
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<GroupRole> GroupRoles { get; set; }
    }
}
