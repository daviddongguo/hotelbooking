using david.hotelbooking.domain.Abstract;
using david.hotelbooking.domain.Concretes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace david.hotelbooking.api.Controllers
{
    [ApiController]
    public class HomeControllers : ControllerBase
    {
        private readonly IUserRepository _userRep;

        public HomeControllers(IUserRepository userRep)
        {
            _userRep = userRep;
        }

        [HttpGet("/")]
        public string index()
        {
            return "welcome Hotel Booking System! --david wu";
        }

        

        [HttpGet("/users")]
        public IActionResult GetAll()
        {
            var result = _userRep.GetAll().FirstOrDefault();
            return Ok(result);
        }

    }
}
