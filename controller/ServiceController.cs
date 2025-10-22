using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Entitiy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using garage_managemet_backend_api.Models;

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

            var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == record.VehicleID);
            if (!vehicleExists)
                return BadRequest(new { message = "VehicleID does not exist." });

            var customerExists = await _context.Appointment.AnyAsync(a => a.AppointmentID == record.AppointmentID);
            if (!customerExists)
                return BadRequest(new { message = "Appointment does not exist." });

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

            var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == record.VehicleID);
            if (!vehicleExists)
                return BadRequest(new { message = "VehicleID does not exist." });

            var appointmentExists = await _context.Appointment.AnyAsync(a => a.AppointmentID == record.AppointmentID);
            if (!appointmentExists)
                return BadRequest(new { message = "AppointmentID does not exist." });

            var mechanicExists = await _context.Mechanic.AnyAsync(m => m.MechanicID == record.MechanicID);
            if (!mechanicExists)
                return BadRequest(new { message = "MechanicID does not exist." });

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

            service.IsDelete = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Service deleted successfully." });
        }

        // =====================================================================
        // 🔹 SERVICE ITEM CRUD SECTION
        // =====================================================================

        // ✅ GET: api/Service/items
        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<ServiceItem>>> GetAllServiceItems()
        {
            var items = await _context.ServiceItems.ToListAsync();
            return Ok(items);
        }

        // ✅ GET: api/Service/items/{id}
        [HttpGet("items/{id}")]
        public async Task<ActionResult<ServiceItem>> GetServiceItemById(int id)
        {
            var item = await _context.ServiceItems.FindAsync(id);
            if (item == null)
                return NotFound(new { message = "Service item not found." });

            return Ok(item);
        }

        // ✅ POST: api/Service/items
        [HttpPost("items")]
        public async Task<ActionResult<ServiceItem>> CreateServiceItem(ServiceItem item)
        {
            if (item == null)
                return BadRequest(new { message = "Invalid service item data." });

            // Validate that the related service exists
            var serviceExists = await _context.Service.AnyAsync(s => s.ServiceID == item.ServiceId && !s.IsDelete);
            if (!serviceExists)
                return BadRequest(new { message = "Invalid ServiceID — service not found." });

            // Optionally validate part_id if you have a Parts table
            // var partExists = await _context.Parts.AnyAsync(p => p.PartID == item.PartId);
            // if (!partExists)
            //     return BadRequest(new { message = "Invalid PartID — part not found." });

            _context.ServiceItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServiceItemById), new { id = item.Id }, item);
        }

        // ✅ PUT: api/Service/items/{id}
        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateServiceItem(int id, ServiceItem item)
        {
            if (id != item.Id)
                return BadRequest(new { message = "Mismatched ServiceItem ID." });

            var existing = await _context.ServiceItems.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Service item not found." });

            existing.ServiceId = item.ServiceId;
            existing.PartId = item.PartId;
            existing.Qty = item.Qty;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Service item updated successfully." });
        }

        // ✅ DELETE: api/Service/items/{id}
        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteServiceItem(int id)
        {
            var item = await _context.ServiceItems.FindAsync(id);
            if (item == null)
                return NotFound(new { message = "Service item not found." });

            _context.ServiceItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Service item deleted successfully." });
        }
    }
}
