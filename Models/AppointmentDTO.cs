using System;

namespace garage_managemet_backend_api.Models;

public class AppointmentDto
{
    public int AppointmentID { get; set; }
    public int CustomerID { get; set; }
    public string? CustomerName { get; set; }
    public int VehicleID { get; set; }
    public string? VehicleName { get; set; }
    public DateTime Date { get; set; }
    public string? Time { get; set; }
    public string? ServiceType { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public int IsDelete { get; set; }
}
