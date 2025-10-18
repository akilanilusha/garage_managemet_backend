using System;

namespace garage_managemet_backend_api.Models;

public class CostResponse
{
    public string Description { get; set; } = string.Empty;
    public decimal TotalCost { get; set; }
}