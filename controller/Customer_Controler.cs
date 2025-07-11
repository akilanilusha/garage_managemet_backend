using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Models;
using garage_managemet_backend_api.Models.CustomerManagement;

namespace garage_managemet_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerReceiveDTO>>> GetCustomers()
        {
            var customers = await _context.Customer
                .Where(c => c.IsDelete == false)
                .Select(c => new CustomerReceiveDTO(
                    c.Id,
                    c.Name,
                    c.Email,
                    c.ContactInfo
                ))
                .ToListAsync();

            return Ok(customers);
        }

        // GET: api/customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerReceiveDTO>> GetCustomer(int id)
        {
            var customer = await _context.Customer
                .Where(c => c.Id == id && !c.IsDelete)
                .Select(c => new CustomerReceiveDTO(
                    c.Id,
                    c.Name,
                    c.Email,
                    c.ContactInfo
                ))
                .FirstOrDefaultAsync();

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // POST: api/customer
        [HttpPost]
        public async Task<ActionResult<CustomerAddUpdateDTO>> CreateCustomer(CustomerAddUpdateDTO dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                ContactInfo = dto.ContactInfo,
                IsDelete = false
            };

            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        // PUT: api/customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerAddUpdateDTO dto)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null || customer.IsDelete)
                return NotFound();

            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.ContactInfo = dto.ContactInfo;

            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        // PATCH: api/customer/{id}/delete
        [HttpPatch("{id}/delete")]
        public async Task<IActionResult> DeleteCustomer(int id, [FromBody] CustomerDeleteDTO dto)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
                return NotFound();

            customer.IsDelete = dto.IsDelete;
            await _context.SaveChangesAsync();

            return Ok(new { deletedCustomerId = customer.Id });
        }
    }
}
