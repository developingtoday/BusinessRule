namespace BusinessRule.Model
{
    public class MembershipProduct : Product
    {
        public MembershipProduct(MembershipTier tier)
        {
            Tier = tier;
        }

        public MembershipProduct()
        {
            Tier = MembershipTier.Bronze;
        }

        public MembershipTier Tier { get; private set; }

        public void UpgradeTier()
        {
            switch (Tier)
            {
                case MembershipTier.Bronze:
                    Tier = MembershipTier.Silver;
                    break;
                case MembershipTier.Silver:
                    Tier = MembershipTier.Gold;
                    break;
            }

            
        }
    }

    public class UpgradeMembershipProduct : Product
    {

    }
}