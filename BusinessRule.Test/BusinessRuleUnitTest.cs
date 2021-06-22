using System;
using System.Linq;
using BusinessRule.Model;
using Xunit;

namespace BusinessRule.Test
{
    public class BusinessRuleUnitTest:IDisposable
    {
        private readonly IPackingSlipService fakePackingSlip;
        private readonly IMembershipService fakeMembershipService;
        public BusinessRuleUnitTest()
        {
            fakePackingSlip = new FakePackingSlipService();
            fakeMembershipService = new FakeMembershipSerivce();
        }

        [Fact]
        public void WhenPhysicalProduct_Should_GeneratePackingSlipForShipping()
        {
            var phyiscalProduct = new PhysicalProduct()
            {
                Id = Guid.NewGuid(),
                Name = "Physical Product No.1"
            };
            var payment = new Payment<PhysicalProduct>()
            {
                Amount = 1000,
                Product = phyiscalProduct
            };

            
            var sut = new RuleEngine(fakePackingSlip,fakeMembershipService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            var packSlips = fakePackingSlip.GetPackingSlips();
            Assert.Contains(packSlips, d =>d.RefId==phyiscalProduct.Id);
        }

        [Fact]
        public void WhenBookProduct_Should_CreateADuplicatePackingSlip()
        {
            var guidProduct = Guid.NewGuid();
            var bookProduct = new BookProduct()
            {
                Id = guidProduct,
                Name = "Book Product No.1"
            };
            var payment = new Payment<BookProduct>()
            {
                Amount = 100,
                Product = bookProduct
            };
            var sut = new RuleEngine(fakePackingSlip, fakeMembershipService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            Assert.True(fakePackingSlip.GetPackingSlips().Count(a=>a.RefId==guidProduct)==2);
        }

        [Fact]
        public void WhenMemberShipProduct_Should_ActivateThatMembership()
        {
            var membproduct = new MembershipProduct(MembershipTier.Bronze)
                {Id = Guid.NewGuid(), Name = "Membership Prod 1"};
           
            var sut = new RuleEngine(fakePackingSlip, fakeMembershipService);
            var response = sut.Execute(new Payment<MembershipProduct>()
            {
                Amount = 100,
                Product = membproduct
            });
            Assert.True(response.IsValid);
            Assert.Contains(fakeMembershipService.GetMembershipProducts(), product => product.Id == membproduct.Id);
        }

        public void Dispose()
        {
            fakeMembershipService.GetMembershipProducts().Clear();
            fakePackingSlip.GetPackingSlips().Clear();

        }
    }
}
