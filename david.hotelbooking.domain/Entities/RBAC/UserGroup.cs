using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.RBAC
{
    public class UserGroup
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int GroupId { get; set; }
        
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User User { get; set; }
        [ForeignKey("GroupId")]
        [JsonIgnore]
        public virtual Group Group { get; set; }
    }
}
