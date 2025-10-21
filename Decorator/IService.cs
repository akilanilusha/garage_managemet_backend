using System;

namespace garage_managemet_backend_api.Decorator;

public interface IService
{
    public string GetDescription();
    public decimal GetCost();
}
