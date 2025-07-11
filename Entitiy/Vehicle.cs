using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//namespace garage_managemet_backend_api.Models;

[Table("Vehicles")]
public class Vehicle
{
    [Key]
    [Column("VehicleID")]
    public int Id { get; set; }

    [Column("LicensePlate")]
    public required string LicensePlate { get; set; }

    [Column("Make")]
    public required string Make { get; set; }

    [Column("Model")]
    public required string Model { get; set; }

    [Column("Year")]
    public required int Year { get; set; }

    [ForeignKey("Customer")]
    public int CustomerID { get; set; }

    [Column("is_delete")]
    public bool IsDelete { get; set; }



}
