using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace david.hotelbooking.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<RBAC_User>>>> GetAllUsers()
        {
            var response = new ServiceResponse<List<RBAC_User>>();
            try
            {
                response.Data = (await _service.GetAllUsers()).ToList();

            }
            catch (System.Exception ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }

            return Ok(response);

        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
