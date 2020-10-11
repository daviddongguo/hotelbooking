using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace david.hotelbooking.domain.Entities.RBAC
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }


    }
}
