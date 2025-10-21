using System;

namespace garage_managemet_backend_api.Models
{
    public class CostRequest
    {
        public List<string>? SelectedItems { get; set; }
        public List<PartRequest>? Parts { get; set; }  // now we include quantity
    }

    public class PartRequest
    {
        public int PartID { get; set; }
        public int Quantity { get; set; } = 1;
    }
}