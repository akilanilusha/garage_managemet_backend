using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Models;
using garage_managemet_backend_api.Models.SupplierManagement;

namespace garage_managemet_backend_api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SupplierController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/supplier
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierReceiveDTO>>> GetSuppliers()
        {
            var suppliers = await _context.Suppliers
                .Where(s => s.IsDelete == false)
                .Select(s => new SupplierReceiveDTO
                {
                    SupplierID = s.SupplierID,
                    Name = s.Name,
                    Phone = s.Phone
                })
                .ToListAsync();

            return Ok(suppliers);
        }

        // GET: api/supplier/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierReceiveDTO>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers
                .Where(s => s.SupplierID == id && s.IsDelete == false)
                .Select(s => new SupplierReceiveDTO
                {
                    SupplierID = s.SupplierID,
                    Name = s.Name,
                    Phone = s.Phone
                })
                .FirstOrDefaultAsync();

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        // POST: api/supplier
        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateSupplier(SupplierAddUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = new Supplier
            {
                Name = dto.Name,
                Phone = dto.Phone,
                IsDelete = false
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSupplier), new { id = supplier.SupplierID }, supplier);
        }

        // PUT: api/supplier/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, SupplierAddUpdateDTO dto)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            supplier.Name = dto.Name;
            supplier.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return Ok(supplier);
        }

        // PATCH: api/supplier/{id}/delete
        [HttpPatch("{id}/delete")]
        public async Task<IActionResult> DeleteSupplier(int id, [FromBody] SupplierDeleteDTO dto)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return NotFound();

            supplier.IsDelete = dto.IsDelete;
            await _context.SaveChangesAsync();

            return Ok(new { deletedSupplierId = supplier.SupplierID });
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(s => s.SupplierID == id);
        }
    }
}
