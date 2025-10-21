using System;

namespace garage_managemet_backend_api.Entitiy;

public class Part
{
    public int PartID { get; set; }
    public string PartName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 1;  // default 1 if not specified
}
