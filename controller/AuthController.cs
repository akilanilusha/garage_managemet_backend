using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Services;
using garage_managemet_backend_api.Models;


namespace garage_managemet_backend_api.controller;

[ApiController]
[Route("api/[controller]")] //http://localhost:5141/api/auth
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }


    [HttpPost("login")]//http://localhost:5141/api/auth/login
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.SingleOrDefault(u => u.UserName == request.UserName);

        if (user == null)
            return Unauthorized("Invalid username");

        if (!user.Role.Equals(request.UserRole, StringComparison.OrdinalIgnoreCase))
            return Unauthorized("Invalid user role");

        if (request.Password != user.PasswordHash)
            return Unauthorized("Invalid password");

        var token = _tokenService.CreateToken(user);
        return Ok(new
        {
            token,
            expiresIn = 12 * 60 * 60, // 12 hours in seconds
            role = user.Role,
            username = user.UserName
        });
    }

}
