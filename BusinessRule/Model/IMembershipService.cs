using System;
using System.Collections.Generic;

namespace BusinessRule.Model
{
    public interface IMembershipService
    {
        void ActivateMembership(MembershipProduct product);

        void UpdgradeMembership(Guid productId);

        List<MembershipProduct> GetMembershipProducts();
    }
}