using david.hotelbooking.domain.Entities.RBAC;
using System;
using System.ComponentModel.DataAnnotations;

namespace david.hotelbooking.domain.Entities.Hotel
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public User Customer { get; set; }
        public User Created_By { get; set; }
        public DateTime Created_At { get; set; }
    }
}
