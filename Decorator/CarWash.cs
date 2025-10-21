using System;

namespace garage_managemet_backend_api.Decorator;

public class CarWash : ServiceDecorator
{
    public CarWash(IService service) : base(service) { }

    public override string GetDescription()
    {
        return _service.GetDescription() + ", Car Wash";
    }

    public override decimal GetCost()
    {
        return _service.GetCost() + 1000;
    }
}