using System;
using garage_managemet_backend_api.Entitiy;

namespace garage_managemet_backend_api.Decorator;

public class PartDecorator : ServiceDecorator
{
    private readonly Part _part;

    public PartDecorator(IService service, Part part) : base(service)
    {
        _part = part;
    }

    public override string GetDescription()
    {
        return $"{base.GetDescription()} + {_part.PartName} x{_part.Quantity}";
    }

    public override decimal GetCost()
    {
        return base.GetCost() + (_part.Price * _part.Quantity);
    }

    // Optional: expose part details for invoice
    public Part GetPart() => _part;
}
