using System;

namespace garage_managemet_backend_api.Decorator
{
    public class BasicService : IService
    {
        private readonly string _description;
        private readonly decimal _cost;

        public BasicService(string description, decimal cost)
        {
            _description = description;
            _cost = cost;
        }

        public string GetDescription()
        {
            return _description;
        }

        public decimal GetCost()
        {
            return _cost;
        }
    }
}
