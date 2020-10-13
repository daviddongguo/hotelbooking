using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace david.hotelbooking.domain.Entities.Hotel
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
