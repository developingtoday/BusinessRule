using System;
using System.Collections.Generic;
using System.Text;
using BusinessRule.Model;

namespace BusinessRule
{
    public class RuleEngine
    {
        private readonly IPackingSlipService _packingSlipService;

        public RuleEngine(IPackingSlipService packingSlipService)
        {
            _packingSlipService = packingSlipService;
        }

        public Result<bool> Execute<T>(Payment<T> payment) where T:Product
        {
            if (typeof(T) == typeof(PhysicalProduct))
            {
               _packingSlipService.GeneratePackingSlip(payment.Product);
            }

            if (typeof(T) == typeof(BookProduct))
            {
                _packingSlipService.DuplicatePackingSlip(payment.Product);
            }

            return new Result<bool>()
            {
                Data = true,
                IsValid = true
            };
        }
    }
}
