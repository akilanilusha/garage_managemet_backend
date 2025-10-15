using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Entitiy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace garage_managemet_backend_api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Service
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetAllServices()
        {
            var services = await _context.Service
                .Where(s => s.IsDelete == false)
                .ToListAsync();

            return Ok(services);
        }

        // ✅ GET: api/Service/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetServiceById(int id)
        {
            var service = await _context.Service.FindAsync(id);

            if (service == null || service.IsDelete == true)
                return NotFound(new { message = "Service record not found." });

            return Ok(service);
        }

        // ✅ POST: api/Service
        [HttpPost]
        public async Task<ActionResult<Service>> CreateService(Service record)
        {
            if (record == null)
                return BadRequest(new { message = "Invalid service data." });

            // 1. Check if VehicleID exists
            var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == record.VehicleID);
            if (!vehicleExists)
                return BadRequest(new { message = "VehicleID does not exist." });

            // 2. Check if CustomerID exists
            var customerExists = await _context.Appointment.AnyAsync(a => a.AppointmentID == record.AppointmentID);
            if (!customerExists)
                return BadRequest(new { message = "Appoinment does not exist." });

            // 3. Check if MechanicID exists
            var mechanicExists = await _context.Mechanic.AnyAsync(m => m.MechanicID == record.MechanicID);
            if (!mechanicExists)
                return BadRequest(new { message = "MechanicID does not exist." });

            try
            {
                record.IsDelete = false;
                _context.Service.Add(record);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetServiceById), new { id = record.ServiceID }, record);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to create service.", error = ex.Message });
            }
        }



        // ✅ PUT: api/Service/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, Service record)
        {
            var existing = await _context.Service.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Service record not found." });

            // Validate related IDs
            var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == record.VehicleID);
            if (!vehicleExists)
                return BadRequest(new { message = "VehicleID does not exist." });

            var appointmentExists = await _context.Appointment.AnyAsync(a => a.AppointmentID == record.AppointmentID);
            if (!appointmentExists)
                return BadRequest(new { message = "AppointmentID does not exist." });

            var mechanicExists = await _context.Mechanic.AnyAsync(m => m.MechanicID == record.MechanicID);
            if (!mechanicExists)
                return BadRequest(new { message = "MechanicID does not exist." });

            // Update fields (do not update ServiceID)
            existing.VehicleID = record.VehicleID;
            existing.AppointmentID = record.AppointmentID;
            existing.MechanicID = record.MechanicID;
            existing.Description = record.Description;
            existing.StartDate = record.StartDate;
            existing.EndDate = record.EndDate;
            existing.PaymentID = record.PaymentID;
            existing.IsDelete = record.IsDelete;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Service updated successfully." });
        }


        // ✅ DELETE: api/Service/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Service.FindAsync(id);
            if (service == null)
                return NotFound(new { message = "Service record not found." });

            // Soft delete
            service.IsDelete = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Service deleted successfully." });
        }
    }
}
