using System;

namespace garage_managemet_backend_api.Models;

public class LoginRequest
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string UserRole { get; set; }
}
