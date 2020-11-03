using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.RBAC
{
    public class UserRole
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User User { get; set; }
        [ForeignKey("RoleId")]
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }
}
