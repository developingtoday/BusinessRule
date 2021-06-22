using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessRule.Model
{
    public class PackingSlip
    {
        public PackingSlip(Guid refId)
        {
            RefId = refId;
        }

        public Guid RefId { get; private set; }

        

    }

    public class Result<T>
    {
        public T Data { get; set; }

        public string Messages { get; set; }

        public bool IsValid { get; set; }

    }


    public interface IPackingSlipService
    {
        void GeneratePackingSlip(Product product);

        void DuplicatePackingSlip(Product product);

        List<PackingSlip> GetPackingSlips();
    }

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
