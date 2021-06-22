using System;
using System.Collections.Generic;
using BusinessRule.Model;

namespace BusinessRule.Service
{
    public interface IMembershipService
    {
        void ActivateMembership(MembershipProduct product);

        void UpdgradeMembership(Guid productId);

        List<MembershipProduct> GetMembershipProducts();
    }
}