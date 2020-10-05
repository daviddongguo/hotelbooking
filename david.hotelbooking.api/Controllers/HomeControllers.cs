using david.hotelbooking.domain.Abstract;
using david.hotelbooking.domain.Concretes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
            var localTime = DateTime.Now.ToString("ddd MMM %d, yyyy  h:mm:ss tt");
            return "welcome Hotel Booking System!\n --david wu \n\t\t" + localTime;
        }



        [HttpGet("/users")]
        public IActionResult GetAll()
        {
            var result = _userRep.GetAll();
            return Ok(result);
        }

    }
}
