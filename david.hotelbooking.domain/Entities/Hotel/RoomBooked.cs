using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace david.hotelbooking.domain.Entities.Hotel
{
    public class RoomBooked
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
    }
}
