using System;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Decorator;
using garage_managemet_backend_api.Entitiy;
using garage_managemet_backend_api.Models;
using Microsoft.EntityFrameworkCore;

namespace garage_managemet_backend_api.Services;

public class CostCalculatorService
{
    private readonly AppDbContext _context;

    public CostCalculatorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CostResponse> CalculateAsync(CostRequest request)
    {
        IService service = new BasicService();

        
        if (request.SelectedItems != null)
        {
            foreach (var addon in request.SelectedItems)
            {
                switch (addon.ToLower())
                {
                    case "engine": service = new EngineTuneUp(service); break;
                    case "wash": service = new CarWash(service); break;
                }
            }
        }

        
        if (request.Parts != null && request.Parts.Count > 0)
        {
            var partIds = request.Parts.Select(p => p.PartID).ToList();
            var _parts = await _context.Items
                .Where(p => partIds.Contains(p.ItemID))
                .ToListAsync();

            foreach (var p in request.Parts)
            {
                var item = _parts.First(x => x.ItemID == p.PartID);
                var part = new Part
                {
                    PartID = item.ItemID,
                    PartName = item.ItemName,
                    Price = item.UnitPrice,
                    Quantity = p.Quantity
                };
                service = new PartDecorator(service, part);
            }
        }

        return new CostResponse
        {
            Description = service.GetDescription(),
            TotalCost = service.GetCost()
        };
    }
}
