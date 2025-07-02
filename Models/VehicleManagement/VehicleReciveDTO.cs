using System;

namespace garage_managemet_backend_api.Models.VehicleManagement;

public class VehicleReciveDTO
{
    public int VehicleID { get; }
    public string LicensePlate { get; }
    public string Make { get; }
    public string Model { get; }
    public int Year { get; }
    public int CustomerID { get; }

    public VehicleReciveDTO(int vehicleID, string licensePlate, string make, string model, int year, int customerID)
    {
        VehicleID = vehicleID;
        LicensePlate = licensePlate;
        Make = make;
        Model = model;
        Year = year;
        CustomerID = customerID;
    }
}
