using System.Collections.Generic;

namespace BusinessRule.Model
{
    public interface IMembershipService
    {
        void ActivateMembership(MembershipProduct product);

        void UpdgradeMembership(MembershipProduct product);

        List<MembershipProduct> GetMembershipProducts();
    }
}