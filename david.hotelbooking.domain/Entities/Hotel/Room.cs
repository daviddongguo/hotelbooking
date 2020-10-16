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
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        [ForeignKey("RoomTypeId")]
        public virtual RoomType RoomType { get; set; }
        public int RoomGroupId { get; set; }
        [ForeignKey("RoomGroupId")]
        public virtual RoomGroup RoomGroup { get; set; }
        [JsonIgnore]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}