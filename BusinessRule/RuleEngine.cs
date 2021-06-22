using System;
using System.Collections.Generic;
using System.Text;
using BusinessRule.Model;

namespace BusinessRule
{
    public class RuleEngine
    {
        public RuleEngine(IPackingSlipService packingSlipService, IMembershipService membershipService)
        {
            _packingSlipService = packingSlipService;
            _membershipService = membershipService;
        }

        private readonly IPackingSlipService _packingSlipService;
        private readonly IMembershipService _membershipService;

        

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

            if (typeof(T) == typeof(MembershipProduct))
            {
                _membershipService.ActivateMembership(payment.Product as MembershipProduct);
            }

            if (typeof(T) == typeof(UpgradeMembershipProduct))
            {
                _membershipService.UpdgradeMembership(payment.Product.Id);
            }

            return new Result<bool>()
            {
                Data = true,
                IsValid = true
            };
        }
    }
}
