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
            var sut = new RuleEngine();
            var response = sut.Execute(payment);
            Assert.True(response!=null);
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
            var sut = new RuleEngine();
            var response = sut.Execute(payment);
            Assert.True(response.Data.Count(a => a.RefId==guidProduct)==2);
        }

    }
}
