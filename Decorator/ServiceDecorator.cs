using System;

namespace garage_managemet_backend_api.Decorator;

public abstract class ServiceDecorator : IService
{
    protected readonly IService _service;

    protected ServiceDecorator(IService service)
    {
        _service = service;
    }

    public virtual string GetDescription()
    {
        return _service.GetDescription();
    }

    public virtual decimal GetCost()
    {
        return _service.GetCost();
    }
}
