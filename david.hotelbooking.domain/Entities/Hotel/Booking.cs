using System;
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
        [ForeignKey("GuestId")]
        [JsonIgnore]
        public virtual Guest Guest { get; set; }

        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        [JsonIgnore]
        public virtual Room Room { get; set; }
        private DateTime _fromDate = DateTime.Now.Date + new TimeSpan(14, 0, 0);
        private DateTime _toDate = DateTime.Now.AddDays(1).Date + new TimeSpan(10, 0, 0);

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MMM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }
            set {
                _fromDate = DefineCheckInTime(value);            
            }
        }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MMM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ToDate {
            get
            {
                return _toDate;                
            }
            set {
                _toDate = DefineCheckOutTime(value);
            }
        }

        private DateTime DefineCheckInTime(DateTime dateTime)
        {
            return dateTime + new TimeSpan(14, 0, 0);
        }

        private DateTime DefineCheckOutTime(DateTime dateTime)
        {
            return dateTime + new TimeSpan(10, 0, 0);
        }

    }
}