using garage_managemet_backend_api.Decorator;
using garage_managemet_backend_api.Entitiy;
using garage_managemet_backend_api.Models;

namespace garage_managemet_backend_api.Services
{
    public class CostCalculatorService
    {
        public async Task<CostResponse> CalculateAsync(CostRequest request)
        {
            string baseDescription = request.BaseServiceDescription ?? "Basic Maintenance Service";
            decimal baseCost = request.BaseServiceCost > 0 ? request.BaseServiceCost : 5000;

            IService service = new BasicService(baseDescription, baseCost);

            if (request.SelectedAddons != null && request.SelectedAddons.Count > 0)
            {
                foreach (var addon in request.SelectedAddons)
                {
                    service = new BasicService(addon.Description, addon.Cost);
                }
            }

            if (request.Parts != null && request.Parts.Count > 0)
            {
                foreach (var p in request.Parts)
                {
                    var part = new Part
                    {
                        PartID = p.PartID,
                        PartName = p.PartName,
                        Price = p.Price,
                        Quantity = p.Quantity,
                    };
                    service = new PartDecorator(service, part);
                }
            }

            return new CostResponse
            {
                Description = service.GetDescription(),
                TotalCost = service.GetCost(),
            };
        }
    }
}
