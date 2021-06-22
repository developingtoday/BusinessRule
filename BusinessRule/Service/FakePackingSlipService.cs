using System.Collections.Generic;
using BusinessRule.Model;

namespace BusinessRule.Service
{
    public class FakePackingSlipService : IPackingSlipService
    {

        private readonly List<PackingSlip> _lst;
        public FakePackingSlipService()
        {
            _lst = new List<PackingSlip>();
        }

        public void GeneratePackingSlip(Product product)
        {
            _lst.Add(new PackingSlip(product.Id)
            {
                Name = product.Name
            });
        }

        public void DuplicatePackingSlip(Product product)
        {
            var found=_lst.Find(a => a.RefId == product.Id);
            if (found == null)
            {
                found = new PackingSlip(product.Id)
                {
                    Name = product.Name
                };
                _lst.Add(found);
                
            }
            _lst.Add(new PackingSlip(found.RefId)
            {
                Name = found.Name
            });
        }

        public List<PackingSlip> GetPackingSlips()
        {
            return _lst;
        }
    }
}