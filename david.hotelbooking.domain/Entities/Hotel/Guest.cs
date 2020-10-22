using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace david.hotelbooking.domain.Entities.Hotel
{
    public class Guest
    {
        [Key]
        public int Id { get; set; } = 0;
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
