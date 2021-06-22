using BusinessRule.Model;
using BusinessRule.Service;

namespace BusinessRule.Logic
{
    public class MembershipProductRule : IExecute<Payment<MembershipProduct>>
    {
        private readonly IMembershipService _membershipService;
        private readonly IMailingService _mailingService;

        public MembershipProductRule(IMembershipService membershipService, IMailingService mailingService)
        {
            _membershipService = membershipService;
            _mailingService = mailingService;
        }

        public void DoStuff(Payment<MembershipProduct> message)
        {
            _membershipService.ActivateMembership(message.Product as MembershipProduct);
            _mailingService.SendMail(new Mail()
            {
                Id = message.Product.Id,
                Msg = "An upgrade was made"
            });
        }
    }
}