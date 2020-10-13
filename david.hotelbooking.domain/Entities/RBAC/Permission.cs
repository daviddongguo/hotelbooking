using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.RBAC
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
