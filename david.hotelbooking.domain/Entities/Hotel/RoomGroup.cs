using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.Hotel
{
    public class RoomGroup
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
