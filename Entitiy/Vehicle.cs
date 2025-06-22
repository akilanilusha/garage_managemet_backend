using System;

namespace garage_managemet_backend_api.Models;

public class Vehicle
{
    public int Id { get; set; }
    public required string LicensePlate { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public required int Year { get; set; }
    
}
