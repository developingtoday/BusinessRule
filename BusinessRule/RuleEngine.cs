using System;
using System.Collections.Generic;
using System.Text;
using BusinessRule.Logic;
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

  

        public RuleEngine(IPackingSlipService packingSlipService, IMembershipService membershipService,
            IMailingService mailingService, IComissionPaymentService comissionPaymentService)
        {
            _packingSlipService = packingSlipService;
            _membershipService = membershipService;
            _mailingService = mailingService;
            _comissionPaymentService = comissionPaymentService;
        }

   

        public Result<bool> Execute<T>(Payment<T> payment) where T : Product
        {
            if (typeof(T) == typeof(PhysicalProduct))
            {
                var rule = new PhysicalProductRule(_packingSlipService, _comissionPaymentService);
                rule.DoStuff(payment as Payment<PhysicalProduct>);
            }

            if (typeof(T) == typeof(BookProduct))
            {
                var rule = new BookProductRule(_packingSlipService, _comissionPaymentService);
                rule.DoStuff(payment as Payment<BookProduct>);
            }

            if (typeof(T) == typeof(MembershipProduct))
            {
                var rule = new MembershipProductRule(_membershipService, _mailingService);
                rule.DoStuff(payment as Payment<MembershipProduct>);
            }

            if (typeof(T) == typeof(VideoProduct))
            {
                var rule = new VideoProductRule(_packingSlipService);
                rule.DoStuff(payment as Payment<VideoProduct>);
            }

            if (typeof(T) == typeof(UpgradeMembershipProduct))
            {
                var rule = new UpgradeMembershipProductRule(_membershipService, _mailingService);
                rule.DoStuff(payment as Payment<UpgradeMembershipProduct>);
            }

            return new Result<bool>()
            {
                Data = true,
                IsValid = true
            };
        }
    }
}