using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.Hotel
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int RoomBookedId { get; set; }
        [ForeignKey("GuestId")]
        [JsonIgnore]
        public virtual Guest Guest { get; set; }
        [ForeignKey("RoomBookedId")]
        public virtual RoomBooked RoomBooked { get; set; }
    }
}