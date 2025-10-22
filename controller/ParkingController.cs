using garage_managemet_backend_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace garage_managemet_backend_api.controller
{
    [Route("api/[controller]")]
    [ApiController]

    public class ParkingController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]  // <-- Allow unauthenticated requests

        public IActionResult Post([FromBody] ParkingStatus status)
        {
            if (status == null)
                return BadRequest("Invalid data");

            Console.WriteLine($"Slot1: {status.Slot1}, Slot2: {status.Slot2}");


            return Ok(new { message = "Data received" });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new { message = "Server is reachable!" });
        }

    }
}
