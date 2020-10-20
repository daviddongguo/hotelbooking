using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace david.hotelbooking.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {

        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }
        // GET: api/<BookingsController>
        [HttpGet]
        public async Task<ServiceResponse<List<BookingEvent>>> GetAllBookings()
        {

            var response = new ServiceResponse<List<BookingEvent>>();
            try
            {
                var eventsList = new List<BookingEvent>();
                var bookingsList = (await _service.GetAllBookings()).ToList();
                foreach (var booking in bookingsList)
                {
                    eventsList.Add(CreateEvent(booking));
                }
                response.Data = eventsList;
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }



        // GET api/<BookingsController>/5
        [HttpGet("{id}")]
        public async Task<ServiceResponse<BookingEvent>> GetBookingById(int id)
        {
            var response = new ServiceResponse<BookingEvent>();
            try
            {
                response.Data = CreateEvent(await _service.GetBookingById(id));
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        // POST api/<BookingsController>
        [HttpPost]
        public async Task<ServiceResponse<Booking>> AddBooking([FromBody] BookingEvent toAddEvent)
        {
            var response = new ServiceResponse<Booking>();
            Int32.TryParse(toAddEvent.Resource, out int roomId);
            DateTime.TryParse(toAddEvent.Start, out DateTime fromDate);
            DateTime.TryParse(toAddEvent.End, out DateTime toDate);
            var guestId = (await _service.GetGuestByEmail(toAddEvent.Text)).Id;

            try
            {
                var booking = new Booking
                {
                    RoomId = roomId,
                    GuestId = guestId,
                    FromDate = fromDate,
                    ToDate = toDate
                };
                var dbBooking = await _service.AddBooking(booking);
                response.Data = booking;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        // PUT api/<BookingsController>/5
        [HttpPatch("{id}")]
        public async Task<string> UpdateBookingDate(int bookingId, [FromBody] BookingEvent toUpdatevalue)
        {
            return await Task.FromResult("Update success");
        }

        // DELETE api/<BookingsController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _service.DeleteBooking(id);
            return;
        }


        private BookingEvent CreateEvent(Booking booking)
        {
            if (booking == null)
            {
                return null;
            }
            return new BookingEvent
            {
                Id = booking.Id.ToString(),
                Text = booking.Guest.Email,
                Start = booking.FromDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                End = booking.ToDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                Resource = booking.RoomId.ToString(),
            };
        }
    }
}
