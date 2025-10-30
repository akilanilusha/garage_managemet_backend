using System;

namespace garage_managemet_backend_api.Models
{
    public class CostRequest
    {
        public string? BaseServiceDescription { get; set; }
        public decimal BaseServiceCost { get; set; }

        public List<AddonRequest>? SelectedAddons { get; set; }
        public List<PartRequest>? Parts { get; set; }
    }

    public class AddonRequest
    {
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public List<string>? SelectedItems { get; set; }
        public List<PartRequest>? Parts { get; set; }  
    }

    public class PartRequest
    {
        public int PartID { get; set; }
        public string PartName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
