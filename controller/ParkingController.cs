using garage_managemet_backend_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace garage_managemet_backend_api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private static ParkingStatus? _latestStatus;

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] ParkingStatus status)
        {
            if (status == null)
                return BadRequest("Invalid data");

            
            _latestStatus = status;

            Console.WriteLine($"Received: Slot1={status.Slot1}, Slot2={status.Slot2}");

            return Ok(new { message = "Data received successfully" });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetStatus()
        {
            if (_latestStatus == null)
                return NotFound(new { message = "No status data available yet" });

            return Ok(new { message = "Latest parking status", data = _latestStatus });
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new { message = "Server is reachable!" });
        }
    }
}
