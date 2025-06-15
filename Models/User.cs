using System;

namespace garage_managemet_backend_api.Models;

public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; } 
}
