using System;
using System.Linq;
using BusinessRule.Model;
using BusinessRule.Service;
using Xunit;

namespace BusinessRule.Test
{
    public class BusinessRuleUnitTest:IDisposable
    {
        private readonly IPackingSlipService _fakePackingSlip;
        private readonly IMembershipService _fakeMembershipService;
        private readonly IMailingService _fakeMailingService;
        private readonly IComissionPaymentService _fakeComissionPaymentService;
        public BusinessRuleUnitTest()
        {
            _fakePackingSlip = new FakePackingSlipService();
            _fakeMembershipService = new FakeMembershipSerivce();
            _fakeMailingService = new MailingService();
            _fakeComissionPaymentService = new FakeComissionPaymentSerivce();
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

            
            var sut = new RuleEngine(_fakePackingSlip,_fakeMembershipService,_fakeMailingService, _fakeComissionPaymentService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            var packSlips = _fakePackingSlip.GetPackingSlips();
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
            var sut = new RuleEngine(_fakePackingSlip, _fakeMembershipService, _fakeMailingService, _fakeComissionPaymentService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            Assert.True(_fakePackingSlip.GetPackingSlips().Count(a=>a.RefId==guidProduct)==2);
        }

        [Fact]
        public void WhenMemberShipProduct_Should_ActivateThatMembership()
        {
            var membproduct = new MembershipProduct(MembershipTier.Bronze)
                {Id = Guid.NewGuid(), Name = "Membership Prod 1"};

            var sut = new RuleEngine(_fakePackingSlip, _fakeMembershipService, _fakeMailingService, _fakeComissionPaymentService);
            var response = sut.Execute(new Payment<MembershipProduct>()
            {
                Amount = 100,
                Product = membproduct
            });
            Assert.True(response.IsValid);
            Assert.Contains(_fakeMembershipService.GetMembershipProducts(), product => product.Id == membproduct.Id);
            Assert.Contains(_fakeMailingService.GetSentMailsSent(), mail => mail.Id == membproduct.Id);
        }

        [Fact]
        public void WhenMemberShipProductUpgrade_Should_ApplyUpgrade()
        {
            var membproduct = new MembershipProduct(MembershipTier.Bronze)
                { Id = Guid.NewGuid(), Name = "Membership Prod 1" };
            _fakeMembershipService.ActivateMembership(membproduct);
            var sut = new RuleEngine(_fakePackingSlip, _fakeMembershipService, _fakeMailingService, _fakeComissionPaymentService);
            var response = sut.Execute(new Payment<UpgradeMembershipProduct>()
            {
                Amount = 100,
                Product = new UpgradeMembershipProduct(){Id = membproduct.Id}
            });
            Assert.True(response.IsValid);  
            Assert.Contains(_fakeMembershipService.GetMembershipProducts(), product => product.Id == membproduct.Id && product.Tier==MembershipTier.Silver);
        }

        [Fact]
        public void WhenMemberShipProductUpgrade_Should_SendNotification()
        {
            var membproduct = new MembershipProduct(MembershipTier.Bronze)
                { Id = Guid.NewGuid(), Name = "Membership Prod 1" };
            _fakeMembershipService.ActivateMembership(membproduct);
            var sut = new RuleEngine(_fakePackingSlip, _fakeMembershipService, _fakeMailingService, _fakeComissionPaymentService);
            var response = sut.Execute(new Payment<UpgradeMembershipProduct>()
            {
                Amount = 100,
                Product = new UpgradeMembershipProduct() { Id = membproduct.Id }
            });
            Assert.True(response.IsValid);
            Assert.Contains(_fakeMembershipService.GetMembershipProducts(), product => product.Id == membproduct.Id && product.Tier == MembershipTier.Silver);
            Assert.Contains(_fakeMailingService.GetSentMailsSent(), mail => mail.Id == membproduct.Id);
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


            var sut = new RuleEngine(_fakePackingSlip, _fakeMembershipService, _fakeMailingService, _fakeComissionPaymentService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            var packSlips = _fakePackingSlip.GetPackingSlips();
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
            var sut = new RuleEngine(_fakePackingSlip, _fakeMembershipService, _fakeMailingService, _fakeComissionPaymentService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            Assert.True(_fakePackingSlip.GetPackingSlips().Count(a => a.RefId == guidProduct) == 2);
            Assert.Contains(_fakeComissionPaymentService.GetComissionPayments(), d =>d.Product.Id==guidProduct);
        }   
        

        public void Dispose()
        {
            _fakeMembershipService.GetMembershipProducts().Clear();
            _fakePackingSlip.GetPackingSlips().Clear();

        }
    }
}
