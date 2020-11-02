using david.hotelbooking.api.SchedulerModels;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace david.hotelbooking.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IBookingService _service;

        public RoomsController(IBookingService service)
        {
            _service = service;
        }
        // GET: api/<RoomsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<List<Resource>>>> GetAllRooms()
        {

            var response = new ServiceResponse<List<Resource>>();
            try
            {
                var roomGroupsList = new List<Resource>();
                var rooms = (await _service.GetAllRooms()).ToList();
                foreach (var room in rooms)
                {
                    var child = new Child
                    {
                        Id = room.Id.ToString(),
                        Name = room.RoomNumber
                    };

                    var Ids = roomGroupsList.Select(g => g.Id);
                    var roomGroupId = "Group" + room.RoomGroupId.ToString();
                    if (Ids.Contains(roomGroupId))
                    {
                        var roomGroup = roomGroupsList.FirstOrDefault(g => g.Id.Equals(roomGroupId));
                        roomGroup.Children.Add(child);
                    }
                    else
                    {
                        var newRoomGroup = new Resource
                        {
                            Children = new List<Child>
                            {
                                child
                            },
                            Name = room.RoomGroup.Name,
                            Id = roomGroupId
                        };
                        roomGroupsList.Add(newRoomGroup);
                    }
                }
                response.Data = roomGroupsList;

            }
            catch (System.Exception ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }

            return Ok(response);
        }

        // GET api/<RoomsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RoomsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RoomsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoomsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
