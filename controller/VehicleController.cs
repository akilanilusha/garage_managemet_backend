using Microsoft.AspNetCore.Authorization;
using garage_managemet_backend_api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using garage_managemet_backend_api.Models;
using Microsoft.EntityFrameworkCore;
using garage_managemet_backend_api.Models.VehicleManagement;

namespace garage_managemet_backend_api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //  Require JWT token for access
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehicleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/vehicle
        [HttpGet]

        public async Task<ActionResult<IEnumerable<VehicleReciveDTO>>> GetVehicles()
        {
            var vehicles = await _context.Vehicles
                                         .Where(v => v.IsDelete == false)
                                         .Select(v => new VehicleReciveDTO(
                                             v.Id,
                                             v.LicensePlate,
                                             v.Make,
                                             v.Model,
                                             v.Year,
                                             v.CustomerID
                                         ))
                                         .ToListAsync();

            return Ok(vehicles);
        }

        // GET: api/vehicle/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleReciveDTO>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles
                                        .Where(v => v.Id == id && v.IsDelete == false)
                                        .Select(v => new VehicleReciveDTO(
                                            v.Id,
                                            v.LicensePlate,
                                            v.Make,
                                            v.Model,
                                            v.Year,
                                            v.CustomerID
                                        ))
                                        .FirstOrDefaultAsync();

            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }

        // POST: api/vehicle
        [HttpPost]
        public async Task<ActionResult<VehicleAddUpdateDTO>> CreateVehicle(VehicleAddUpdateDTO vehicleDto)
        {
            var existingVehicle = await _context.Vehicles
                                                .Where(v => v.LicensePlate == vehicleDto.LicensePlate && v.IsDelete == false)
                                                .FirstOrDefaultAsync();

            if (existingVehicle != null)
            {
                return Conflict($"A vehicle with LicensePlate '{vehicleDto.LicensePlate}' already exists and is active.");
            }

            var customer = await _context.Customer.FindAsync(vehicleDto.CustomerID);
            if (customer == null)
            {
                return NotFound($"Customer with ID {vehicleDto.CustomerID} not found.");
            }
            var vehicle = new Vehicle
            {
                LicensePlate = vehicleDto.LicensePlate,
                Make = vehicleDto.Make,
                Model = vehicleDto.Model,
                Year = vehicleDto.Year,
                CustomerID = vehicleDto.CustomerID
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return Ok(vehicle);

        }

        // PUT: api/vehicle/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleAddUpdateDTO dto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return NotFound($"Vehicle with ID {id} not found.");

            var customerExists = await _context.Customer.AnyAsync(c => c.Id == dto.CustomerID);
            if (!customerExists)
                return NotFound($"Customer with ID {dto.CustomerID} not found.");

            // Update the existing vehicle
            vehicle.LicensePlate = dto.LicensePlate;
            vehicle.Make = dto.Make;
            vehicle.Model = dto.Model;
            vehicle.Year = dto.Year;
            vehicle.CustomerID = dto.CustomerID;

            await _context.SaveChangesAsync();

            return Ok(vehicle);
        }


        // DELETE: api/vehicle/{id}/delete
        [HttpPatch("{id}/delete")]
        public async Task<IActionResult> DeleteVehicle(int id, [FromBody] VehicleDeleteDTO dto)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();

            vehicle.IsDelete = dto.IsDelete;
            await _context.SaveChangesAsync();

            return Ok(new { deletedVehicleId = vehicle.Id });
        }

    }
}
