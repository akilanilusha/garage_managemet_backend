using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garage_managemet_backend_api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // You can remove this if you want open access
    public class MechanicController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MechanicController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Mechanic
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mechanic>>> GetAllMechanics()
        {
            var mechanics = await _context.Mechanic
                .Where(m => m.is_delete == false)
                .ToListAsync();

            return Ok(mechanics);
        }

        // ✅ GET: api/Mechanic/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Mechanic>> GetMechanicById(int id)
        {
            var mechanic = await _context.Mechanic.FindAsync(id);
            if (mechanic == null || mechanic.is_delete)
                return NotFound(new { message = "Mechanic not found." });

            return Ok(mechanic);
        }

        // ✅ POST: api/Mechanic
        [HttpPost]
        public async Task<ActionResult<Mechanic>> CreateMechanic(Mechanic mechanic)
        {
            if (mechanic == null)
                return BadRequest(new { message = "Invalid mechanic data." });

            // Check if NIC already exists
            var existingNIC = await _context.Mechanic.AnyAsync(m => m.nic_number == mechanic.nic_number && m.is_delete == false);
            if (existingNIC)
                return BadRequest(new { message = "NIC number already exists." });

            _context.Mechanic.Add(mechanic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMechanicById), new { id = mechanic.MechanicID }, mechanic);
        }

        // ✅ PUT: api/Mechanic/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMechanic(int id, Mechanic mechanic)
        {
            var existing = await _context.Mechanic.FindAsync(id);
            if (existing == null || existing.is_delete)
                return NotFound(new { message = "Mechanic not found." });

            existing.name = mechanic.name;
            existing.nic_number = mechanic.nic_number;
            existing.phone = mechanic.phone;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Mechanic updated successfully." });
        }

        // ✅ DELETE (soft): api/Mechanic/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMechanic(int id)
        {
            var mechanic = await _context.Mechanic.FindAsync(id);
            if (mechanic == null || mechanic.is_delete)
                return NotFound(new { message = "Mechanic not found." });

            mechanic.is_delete = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mechanic deleted successfully." });
        }

        // ✅ SEARCH: api/Mechanic/search?query=someText
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Mechanic>>> SearchMechanic([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { message = "Search query cannot be empty." });

            var mechanics = await _context.Mechanic
                .Where(m => !m.is_delete &&
                            (m.name.Contains(query) ||
                             m.nic_number.Contains(query)))
                .ToListAsync();

            if (mechanics.Count == 0)
                return NotFound(new { message = "No matching mechanics found." });

            return Ok(mechanics);
        }
    }
}
