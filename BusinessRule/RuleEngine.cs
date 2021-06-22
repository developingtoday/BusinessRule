using System;
using System.Collections.Generic;
using System.Text;
using BusinessRule.Model;

namespace BusinessRule
{
    public class RuleEngine
    {


        private readonly IPackingSlipService _packingSlipService;
        private readonly IMembershipService _membershipService;
        private readonly IMailingService _mailingService;

        public RuleEngine(IPackingSlipService packingSlipService, IMembershipService membershipService, IMailingService mailingService)
        {
            _packingSlipService = packingSlipService;
            _membershipService = membershipService;
            _mailingService = mailingService;
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

            if (typeof(T) == typeof(MembershipProduct))
            {
                _membershipService.ActivateMembership(payment.Product as MembershipProduct);
                _mailingService.SendMail(new Mail()
                {
                    Id = payment.Product.Id,
                    Msg = "An upgrade was made"
                });
            }

            if (typeof(T) == typeof(UpgradeMembershipProduct))
            {
                _membershipService.UpdgradeMembership(payment.Product.Id);
                _mailingService.SendMail(new Mail()
                {
                    Id = payment.Product.Id,
                    Msg = "An upgrade was made"
                });
            }

            return new Result<bool>()
            {
                Data = true,
                IsValid = true
            };
        }
    }
}
