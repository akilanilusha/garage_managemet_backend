using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Entitiy;
using garage_managemet_backend_api.Models.CustomerManagement;

namespace garage_managemet_backend_api.controller
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
                .Select(c => new CustomerReceiveDTO
                {
                    CustomerID = c.CustomerID,
                    Name = c.name,
                    Phone = c.phone,
                    Email = c.email
                })
                .ToListAsync();

            return Ok(customers);
        }

        // GET: api/customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerReceiveDTO>> GetCustomer(int id)
        {
            var customer = await _context.Customer
                .Where(c => c.CustomerID == id)
                .Select(c => new CustomerReceiveDTO
                {
                    CustomerID = c.CustomerID,
                    Name = c.name,
                    Phone = c.phone,
                    Email = c.email
                })
                .FirstOrDefaultAsync();

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // POST: api/customer
        [HttpPost]
        public async Task<ActionResult> CreateCustomer([FromBody] CustomerAddUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = new Customer
            {
                name = dto.Name,
                phone = dto.Phone,
                email = dto.Email
            };

            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        // PUT: api/customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerAddUpdateDTO dto)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            customer.name = dto.Name;
            customer.phone = dto.Phone;
            customer.email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        // DELETE: api/customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
                return NotFound();

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(new { deletedCustomerId = id });
        }
    }
}
