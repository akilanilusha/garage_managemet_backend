using System;
using Microsoft.AspNetCore.Mvc;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Services;
using garage_managemet_backend_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace garage_managemet_backend_api.controller;
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
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _context.Entry(user).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
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

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteUsers(int id)
    //{
    //    var user = awit _context.Appointment.FindAsync(id);
    //    if (user == null)
    //    {
    //        return NotFound();
    //    }
    //    _context.Users.Remove(user);
    //    await _context.SaveChangesAsync();
    //    return Ok(new { deleteUserId = UserManagementController.Id });
    //}

    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<User>>> SearchUsers(string searchTerm)
    //{
    //    if (string.IsNullOrEmpty(searchTerm))
    //    {
    //        return BadRequest("Search term cannot be null or empty.");
    //    }
    //    var users = await _context.Users
    //        .Where(u => u.UserName.Contains(searchTerm) || u.Role.Contains(searchTerm))
    //        .ToListAsync();
    //    if (users.Count == 0)
    //    {
    //        return NotFound("No users found matching the search term.");
    //    }
    //    return Ok(users);
    //}

    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    //{
    //    var users = await _context.Users.ToListAsync();
    //    return Ok(users);
    //}

    public bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

}
