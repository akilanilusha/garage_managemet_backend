using System;

namespace garage_managemet_backend_api.Decorator;

public class EngineTuneUp : ServiceDecorator
{
    public EngineTuneUp(IService service) : base(service) { }

    public override string GetDescription()
    {
        return _service.GetDescription() + ", Engine Tune-Up";
    }

    public override decimal GetCost()
    {
        return _service.GetCost() + 2500;
    }
}