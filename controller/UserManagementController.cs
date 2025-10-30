using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace garage_managemet_backend_api.controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserManagementController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest("User ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound($"User with ID {id} not found.");
                else
                    throw;
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { deletedUserId = id });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
