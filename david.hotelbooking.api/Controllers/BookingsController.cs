using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
                if (booking == null)
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
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceResponse<Booking>>> AddBooking([FromBody] BookingEvent toAddEvent)
        {
            var response = new ServiceResponse<Booking>();
            Int32.TryParse(toAddEvent.Resource, out int roomId);
            DateTime.TryParse(toAddEvent.Start, out DateTime fromDate);
            DateTime.TryParse(toAddEvent.End, out DateTime toDate);
            var guest = (await _service.GetGuestByEmail(toAddEvent.Text));
            if(guest == null){
                try
                {
                   guest = await _service.AddGuest(toAddEvent.Text, "");
                }
                catch (System.Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }

            try
            {
                var booking = new Booking
                {
                    RoomId = roomId,
                    GuestId = guest.Id,
                    FromDate = fromDate,
                    ToDate = toDate
                };
                var dbBooking = await _service.AddBooking(booking);
                response.Data = booking;
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, response);

            }
            catch (Exception e)
            {
                response.Message = e.Message;
                return BadRequest(response);
            }
        }

        // PUT api/<BookingsController>/5
        [HttpPatch("{id}")]
        public async Task<string> UpdateBookingDate(int bookingId, [FromBody] BookingEvent toUpdatevalue)
        {
            return await Task.FromResult("Update success");
        }

        // DELETE api/<BookingsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            var toDateDbBooking = await _service.GetBookingById(id);
            if (toDateDbBooking == null)
            {
                return NotFound();
            }

            try
            {
                await _service.DeleteBooking(id);
                return NoContent();
            }
            catch (System.Exception e)
            {
                //TODO: Hide Internal server error when deployed.
                return StatusCode(500, e.Message);
            }
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
