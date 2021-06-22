using BusinessRule.Model;
using BusinessRule.Service;

namespace BusinessRule.Logic
{
    public class UpgradeMembershipProductRule : IExecute<Payment<UpgradeMembershipProduct>>
    {
        private readonly IMembershipService _membershipService;
        private readonly IMailingService _mailingService;

        public UpgradeMembershipProductRule(IMembershipService membershipService, IMailingService mailingService)
        {
            _membershipService = membershipService;
            _mailingService = mailingService;
        }

        public void DoStuff(Payment<UpgradeMembershipProduct> message)
        {
            _membershipService.UpdgradeMembership(message.Product.Id);
            _mailingService.SendMail(new Mail()
            {
                Id = message.Product.Id,
                Msg = "An upgrade was made"
            });
        }
    }
}