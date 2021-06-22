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
        private readonly IMailingService fakeMailingService;
        private readonly IComissionPaymentService fakeComissionPaymentService;
        public BusinessRuleUnitTest()
        {
            fakePackingSlip = new FakePackingSlipService();
            fakeMembershipService = new FakeMembershipSerivce();
            fakeMailingService = new MailingService();
            fakeComissionPaymentService = new FakeComissionPaymentSerivce();
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

            
            var sut = new RuleEngine(fakePackingSlip,fakeMembershipService,fakeMailingService, fakeComissionPaymentService);
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
            var sut = new RuleEngine(fakePackingSlip, fakeMembershipService, fakeMailingService, fakeComissionPaymentService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            Assert.True(fakePackingSlip.GetPackingSlips().Count(a=>a.RefId==guidProduct)==2);
        }

        [Fact]
        public void WhenMemberShipProduct_Should_ActivateThatMembership()
        {
            var membproduct = new MembershipProduct(MembershipTier.Bronze)
                {Id = Guid.NewGuid(), Name = "Membership Prod 1"};

            var sut = new RuleEngine(fakePackingSlip, fakeMembershipService, fakeMailingService, fakeComissionPaymentService);
            var response = sut.Execute(new Payment<MembershipProduct>()
            {
                Amount = 100,
                Product = membproduct
            });
            Assert.True(response.IsValid);
            Assert.Contains(fakeMembershipService.GetMembershipProducts(), product => product.Id == membproduct.Id);
            Assert.Contains(fakeMailingService.GetSentMailsSent(), mail => mail.Id == membproduct.Id);
        }

        [Fact]
        public void WhenMemberShipProductUpgrade_Should_ApplyUpgrade()
        {
            var membproduct = new MembershipProduct(MembershipTier.Bronze)
                { Id = Guid.NewGuid(), Name = "Membership Prod 1" };
            fakeMembershipService.ActivateMembership(membproduct);
            var sut = new RuleEngine(fakePackingSlip, fakeMembershipService, fakeMailingService, fakeComissionPaymentService);
            var response = sut.Execute(new Payment<UpgradeMembershipProduct>()
            {
                Amount = 100,
                Product = new UpgradeMembershipProduct(){Id = membproduct.Id}
            });
            Assert.True(response.IsValid);  
            Assert.Contains(fakeMembershipService.GetMembershipProducts(), product => product.Id == membproduct.Id && product.Tier==MembershipTier.Silver);
        }

        [Fact]
        public void WhenMemberShipProductUpgrade_Should_SendNotification()
        {
            var membproduct = new MembershipProduct(MembershipTier.Bronze)
                { Id = Guid.NewGuid(), Name = "Membership Prod 1" };
            fakeMembershipService.ActivateMembership(membproduct);
            var sut = new RuleEngine(fakePackingSlip, fakeMembershipService, fakeMailingService, fakeComissionPaymentService);
            var response = sut.Execute(new Payment<UpgradeMembershipProduct>()
            {
                Amount = 100,
                Product = new UpgradeMembershipProduct() { Id = membproduct.Id }
            });
            Assert.True(response.IsValid);
            Assert.Contains(fakeMembershipService.GetMembershipProducts(), product => product.Id == membproduct.Id && product.Tier == MembershipTier.Silver);
            Assert.Contains(fakeMailingService.GetSentMailsSent(), mail => mail.Id == membproduct.Id);
        }

        [Fact]
        public void WhenPaymentIsVideoLearningSki_Should_AddFirstAidInPackingSlip()
        {
            var physicalProduct = new VideoProduct()
            {
                Id = Guid.NewGuid(),
                Name = "Learning To Ski"
            };
            var payment = new Payment<VideoProduct>()
            {
                Amount = 1000,
                Product = physicalProduct
            };


            var sut = new RuleEngine(fakePackingSlip, fakeMembershipService, fakeMailingService, fakeComissionPaymentService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            var packSlips = fakePackingSlip.GetPackingSlips();
            Assert.Contains(packSlips, d => d.RefId == physicalProduct.Id);
            Assert.Contains(packSlips, d => d.Name.Equals("First Aid",StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public void WhenPaymentProductOrBook_Should_GenerateCommisionPayment()  
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
            var sut = new RuleEngine(fakePackingSlip, fakeMembershipService, fakeMailingService, fakeComissionPaymentService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            Assert.True(fakePackingSlip.GetPackingSlips().Count(a => a.RefId == guidProduct) == 2);
            Assert.Contains(fakeComissionPaymentService.GetComissionPayments(), d =>d.Product.Id==guidProduct);
        }   
        

        public void Dispose()
        {
            fakeMembershipService.GetMembershipProducts().Clear();
            fakePackingSlip.GetPackingSlips().Clear();

        }
    }
}
