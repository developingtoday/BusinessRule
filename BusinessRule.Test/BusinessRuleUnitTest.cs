using System;
using System.Linq;
using BusinessRule.Model;
using Xunit;

namespace BusinessRule.Test
{
    public class BusinessRuleUnitTest
    {
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

            var packingService = new FakePackingSlipService();
            var sut = new RuleEngine(packingService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            var packSlips = packingService.GetPackingSlips();
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
            var packingService = new FakePackingSlipService();
            var sut = new RuleEngine(packingService);
            var response = sut.Execute(payment);
            Assert.True(response.IsValid);
            Assert.True(packingService.GetPackingSlips().Count(a=>a.RefId==guidProduct)==2);
        }

    }
}
