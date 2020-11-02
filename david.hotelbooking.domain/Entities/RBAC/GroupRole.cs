using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.RBAC
{
    public class GroupRole
    {
        public int GroupId { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("GroupId")]
        [JsonIgnore]
        public virtual Group Group { get; set; }
        [ForeignKey("RoleId")]
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }
}
