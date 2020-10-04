using Microsoft.AspNetCore.Mvc;

namespace david.hotelbooking.api.Controllers
{
    [ApiController]
    public class HomeControllers : ControllerBase
    {
        [HttpGet("/")]
        public string index()
        {
            return "welcome Hotel Booking System! --david wu";
        }

    }
}
