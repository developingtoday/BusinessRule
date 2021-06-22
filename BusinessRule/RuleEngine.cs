using System;
using System.Collections.Generic;
using System.Text;
using BusinessRule.Model;
using BusinessRule.Service;

namespace BusinessRule
{
    public class RuleEngine
    {


        private readonly IPackingSlipService _packingSlipService;
        private readonly IMembershipService _membershipService;
        private readonly IMailingService _mailingService;
        private readonly IComissionPaymentService _comissionPaymentService;


        public RuleEngine(IPackingSlipService packingSlipService, IMembershipService membershipService, IMailingService mailingService, IComissionPaymentService comissionPaymentService)
        {
            _packingSlipService = packingSlipService;
            _membershipService = membershipService;
            _mailingService = mailingService;
            _comissionPaymentService = comissionPaymentService;
        }

        public Result<bool> Execute<T>(Payment<T> payment) where T:Product
        {
            if (typeof(T) == typeof(PhysicalProduct))
            {
               _packingSlipService.GeneratePackingSlip(payment.Product);
               _comissionPaymentService.GenerateComissionPayment(payment.Product);
            }

            if (typeof(T) == typeof(BookProduct))
            {
                _packingSlipService.DuplicatePackingSlip(payment.Product);
                _comissionPaymentService.GenerateComissionPayment(payment.Product);

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

            if (typeof(T) == typeof(VideoProduct))
            {
                _packingSlipService.GeneratePackingSlip(payment.Product);
                if (payment.Product.Name.Equals("Learning to Ski",StringComparison.InvariantCultureIgnoreCase))
                {
                    _packingSlipService.GeneratePackingSlip(new VideoProduct()
                    {
                        Id = Guid.NewGuid(),
                        Name = "First Aid"
                    });
                }
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
