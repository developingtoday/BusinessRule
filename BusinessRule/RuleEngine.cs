using System;
using System.Collections.Generic;
using System.Text;
using BusinessRule.Model;

namespace BusinessRule
{
    public class RuleEngine
    {
        public Result<List<PackingSlip>> Execute<T>(Payment<T> payment) where T:Product
        {
            if (typeof(T) == typeof(PhysicalProduct))
            {
                return new Result<List<PackingSlip>>()
                {
                    Data = new List<PackingSlip>()
                    {
                        new PackingSlip(payment.Product.Id)
                    }
                };
            }

            if (typeof(T) == typeof(BookProduct))
            {
                return new Result<List<PackingSlip>>()
                {
                    Data = new List<PackingSlip>()
                    {
                        new PackingSlip(payment.Product.Id), new PackingSlip(payment.Product.Id)
                    }
                };
            }

            return null;
        }
    }
}
