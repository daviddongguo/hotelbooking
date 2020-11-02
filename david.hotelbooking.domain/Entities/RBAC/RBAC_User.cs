using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace david.hotelbooking.domain.Entities.RBAC
{
    public class RBAC_User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }


    }
}
