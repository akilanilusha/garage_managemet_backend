using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Models;

namespace garage_managemet_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointments = await _context.Appointment
                .Where(a => a.IsDelete == 0)
                .ToListAsync();

            return Ok(appointments);
        }

        //  GET all confiremed
        [HttpGet("confirmed")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetConfirmedAppointments()
        {
            var appointments = await _context.Appointment
                .Where(a => a.IsDelete == 0 && a.Status == "Confirmed")
                .ToListAsync();

            return Ok(appointments);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null || appointment.IsDelete == 1)
            {
                return NotFound();
            }

            return Ok(appointment);
        }



        // ✅ CREATE
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Appointment.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentID }, appointment);
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] Appointment appointment)
        {
            if (id != appointment.AppointmentID)
            {
                return BadRequest("Appointment ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // ✅ SOFT DELETE (mark as deleted)
        [HttpPatch("{id}/delete")]
        public async Task<IActionResult> SoftDeleteAppointment(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.IsDelete = 1;
            await _context.SaveChangesAsync();

            return Ok(new { deletedAppointmentId = id });
        }

        // ✅ HARD DELETE (optional)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointment.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { deletedAppointmentId = id });
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.AppointmentID == id);
        }
    }
}
