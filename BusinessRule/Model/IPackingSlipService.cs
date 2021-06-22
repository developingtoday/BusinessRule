using System.Collections.Generic;

namespace BusinessRule.Model
{
    public interface IPackingSlipService
    {
        void GeneratePackingSlip(Product product);

        void DuplicatePackingSlip(Product product);

        List<PackingSlip> GetPackingSlips();
    }
}