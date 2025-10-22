using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Entitiy;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _context.Customer
                                          .Where(c => c.IsDelete == false)
                                          .ToListAsync();
            return Ok(customers);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer
                                         .Where(c => c.CustomerID == id && c.IsDelete == false)
                                         .FirstOrDefaultAsync();

            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            return Ok(customer);
        }

        
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer customer)
        {
            
            var existingCustomer = await _context.Customer
                .FirstOrDefaultAsync(c => c.email == customer.email && c.IsDelete == false);

            if (existingCustomer != null)
                return Conflict($"Customer with email '{customer.email}' already exists.");

            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerID }, customer);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null || customer.IsDelete)
                return NotFound($"Customer with ID {id} not found.");

            customer.name = updatedCustomer.name;
            customer.phone = updatedCustomer.phone;
            customer.email = updatedCustomer.email;

            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        
        [HttpPatch("{id}/delete")]
        public async Task<IActionResult> DeleteCustomer(int id, [FromBody] bool isDelete)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            customer.IsDelete = isDelete;
            await _context.SaveChangesAsync();

            return Ok(new { deletedCustomerId = id, customer.IsDelete });
        }
    }
}
