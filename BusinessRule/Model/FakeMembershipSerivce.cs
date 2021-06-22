using System.Collections.Generic;

namespace BusinessRule.Model
{
    public class FakeMembershipSerivce: IMembershipService
    {
        private readonly List<MembershipProduct> products;

        public FakeMembershipSerivce()
        {
            products = new List<MembershipProduct>();
        }

        public void ActivateMembership(MembershipProduct product)
        {
            products.Add(product);
        }

        public void UpdgradeMembership(MembershipProduct product)
        {
            var found = products.Find(a => a.Id == product.Id);
            found.UpgradeTier();
            
        }

        public List<MembershipProduct> GetMembershipProducts()
        {
            return products;
        }
    }
}