using System.Collections.Generic;

namespace BusinessRule.Model
{
    public class FakePackingSlipService : IPackingSlipService
    {

        private readonly List<PackingSlip> lst;
        public FakePackingSlipService()
        {
            lst = new List<PackingSlip>();
        }

        public void GeneratePackingSlip(Product product)
        {
            lst.Add(new PackingSlip(product.Id));
        }

        public void DuplicatePackingSlip(Product product)
        {
            var found=lst.Find(a => a.RefId == product.Id);
            if (found == null)
            {
                found = new PackingSlip(product.Id);
                lst.Add(found);
                
            }
            lst.Add(new PackingSlip(found.RefId));
        }

        public List<PackingSlip> GetPackingSlips()
        {
            return lst;
        }
    }
}