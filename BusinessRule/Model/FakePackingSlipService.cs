using System.Collections.Generic;

namespace BusinessRule.Model
{
    public class FakePackingSlipService : IPackingSlipService
    {

        private readonly List<PackingSlip> lst;
        public FakePackingSlipService()
        {
            lst = new List<PackingSlip>(0);
        }

        public void GeneratePackingSlip(Product product)
        {
            lst.Add(new PackingSlip(product.Id));
        }

        public void DuplicatePackingSlip(Product product)
        {
            var found=lst.Find(a => a.RefId == product.Id);
            lst.Add(new PackingSlip(found.RefId));
        }

        public List<PackingSlip> GetPackingSlips()
        {
            return lst;
        }
    }
}