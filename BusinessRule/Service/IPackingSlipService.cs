using System.Collections.Generic;
using BusinessRule.Model;

namespace BusinessRule.Service
{
    public interface IPackingSlipService
    {
        void GeneratePackingSlip(Product product);

        void DuplicatePackingSlip(Product product);

        List<PackingSlip> GetPackingSlips();
    }
}