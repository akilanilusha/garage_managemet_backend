using System;

namespace garage_managemet_backend_api.Decorator;

public class BasicService: IService
{
 public string GetDescription()
    {
        return "Basic Maintenance Service";
    }

    public decimal GetCost()
    {
        return 5000;
    }
}
