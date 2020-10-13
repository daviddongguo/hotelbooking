using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.Hotel
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int RoomTypeId { get; set; }
        [ForeignKey("RoomTypeId")]
        public virtual Room RoomType { get; set; }
        public virtual ICollection<RoomBooked>   RoomBookeds { get; set; }
    }
}