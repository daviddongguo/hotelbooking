using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<ServiceResponse<List<BookingEvent>>>> GetAllBookings()
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
                response.Message = ex.Message;
                return NotFound(response);
            }

            return Ok(response);
        }

        // GET api/<BookingsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<BookingEvent>>> GetBookingById(int id)
        {
            var response = new ServiceResponse<BookingEvent>();
            try
            {
                var booking = await _service.GetBookingById(id);
                if(booking == null)
                {
                    response.Message = $"No Booking(id = {id})";
                    return NotFound(response);
                }

                response.Data = CreateEvent(booking);
            }
            catch (System.Exception ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }

            return Ok(response);
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
        public async Task<ServiceResponse<int>> Delete(int id)
        {
            var response = new ServiceResponse<int>();
            try
            {
                response.Data = await _service.DeleteBooking(id);
            }
            catch (System.Exception e)
            {
                response.Message = e.Message;
            }
            return response;
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
