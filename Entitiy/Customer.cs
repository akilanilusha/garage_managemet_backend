using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_managemet_backend_api.Entitiy;

[Table("Customer")]
public class Customer
{
    public int CustomerID { get; set; }
    public required string name { get; set; }
    public required string phone { get; set; }
    public required string email { get; set; }
    public bool IsDelete { get; set; } = false;

}
