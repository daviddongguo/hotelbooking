using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.Hotel;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ServiceResponse<List<Event>>> GetAllBookings()
        {

            var response = new ServiceResponse<List<Event>>();
            try
            {
                var eventsList = new List<Event>();
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
        public async Task<ServiceResponse<Event>> GetBookingById(int id)
        {
            var response = new ServiceResponse<Event>();
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
        public async Task<string> AddBooking([FromBody] Event newEvent)
        {
            return await Task.FromResult("Add success");
        }

        // PUT api/<BookingsController>/5
        [HttpPatch("{id}")]
        public async Task<string> UpdateBookingDate(int id, [FromBody] Event toUpdatevalue)
        {
            return await Task.FromResult("Update success");
        }

        // DELETE api/<BookingsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        private Event CreateEvent(Booking booking)
        {
            if (booking == null)
            {
                return null;
            }
            return new Event
            {
                Id = booking.Id.ToString(),
                Text = booking.Guest?.Name,
                Start = booking.FromDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                End = booking.ToDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                Resource = booking.RoomId.ToString(),
            };
        }
    }
}
