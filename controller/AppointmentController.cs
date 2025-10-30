using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
        {
            var connection = _context.Database.GetDbConnection();
            var appointments = new List<AppointmentDto>();

            try
            {
                await connection.OpenAsync();
                using var command = connection.CreateCommand();

                command.CommandText =
                    @"
            SELECT 
                a.AppointmentID,
                a.CustomerID,
                c.name AS CustomerName,
                a.VehicleID,
                v.LicensePlate AS VehicleName,
                a.Date,
                a.Time,
                a.ServiceType,
                a.Description,
                a.Status,
                a.is_delete AS IsDelete
            FROM Appointment a
            INNER JOIN Customer c ON a.CustomerID = c.CustomerID
            INNER JOIN Vehicles v ON a.VehicleID = v.VehicleID
            WHERE a.is_delete = 0;";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var dto = new AppointmentDto
                    {
                        AppointmentID = reader.GetInt32(reader.GetOrdinal("AppointmentID")),
                        CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                        CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                        VehicleID = reader.GetInt32(reader.GetOrdinal("VehicleID")),
                        VehicleName = reader.GetString(reader.GetOrdinal("VehicleName")),
                        Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                        Time = reader.IsDBNull(reader.GetOrdinal("Time"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Time")),
                        ServiceType = reader.IsDBNull(reader.GetOrdinal("ServiceType"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("ServiceType")),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Description")),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status"))
                            ? null
                            : reader.GetString(reader.GetOrdinal("Status")),
                        IsDelete = reader.GetInt32(reader.GetOrdinal("IsDelete")),
                    };

                    appointments.Add(dto);
                }
            }
            finally
            {
                await connection.CloseAsync();
            }

            return Ok(appointments);
        }

        [HttpGet("confirmed")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetConfirmedAppointments()
        {
            var appointments = await _context
                .Appointment.Where(a => a.IsDelete == 0 && a.Status == "Confirmed")
                .ToListAsync();

            return Ok(appointments);
        }

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

        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment(
            [FromBody] Appointment appointment
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Appointment.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAppointment),
                new { id = appointment.AppointmentID },
                appointment
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(
            int id,
            [FromBody] Appointment appointment
        )
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
