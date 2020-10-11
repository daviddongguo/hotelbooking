using david.hotelbooking.domain.Abstract;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace david.hotelbooking.api.Controllers
{
    [ApiController]
    public class HomeControllers : ControllerBase
    {
        private readonly IUserRepository _userRep;
        private readonly IUserService _service;

        public HomeControllers(IUserRepository userRep, IUserService service)
        {
            _userRep = userRep;
            _service = service;
        }

        [HttpGet("/")]
        public string index()
        {
            var localTime = DateTime.Now.ToString("ddd MMM %d, yyyy  h:mm:ss tt");
            return "welcome Hotel Booking System!\n --david wu \n\t\t" + localTime;
        }



        [HttpGet("/testusers")]
        public IActionResult GetAll()
        {
            var result = _userRep.GetAllUsers().Select(r => new
            {
                Id = r.Id,
                Name = r.Email,
                Roles = r.UserRoles.Select(rr => new
                {
                    Id = rr.Role.Id,
                    Role = rr.Role.Name,
                    Permissions = rr.Role.RolePermissions.Select(rrr => new
                    {
                        Id = rrr.Permission.Id,
                        Permission = rrr.Permission.Name
                    })
                })
            });
            return Ok(result);
        }

        [HttpGet("/testuser")]
        public IActionResult GetSingleUser()
        {
            var result = _service.GetSingleUser(1).GetAwaiter().GetResult().UserRoles
            .Select(r => r.RoleId);
            return Ok(result);
        }

    }
}
