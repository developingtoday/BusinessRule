using System;
using System.Collections.Generic;
using BusinessRule.Model;

namespace BusinessRule.Service
{
    public class FakeMembershipSerivce: IMembershipService
    {
        private readonly List<MembershipProduct> _products;

        public FakeMembershipSerivce()
        {
            _products = new List<MembershipProduct>();
        }

        public void ActivateMembership(MembershipProduct product)
        {
            _products.Add(product);
        }

        public void UpdgradeMembership(Guid productId)
        {
            var found = _products.Find(a =>productId==a.Id);
            found.UpgradeTier();
            
        }

        public List<MembershipProduct> GetMembershipProducts()
        {
            return _products;
        }
    }
}