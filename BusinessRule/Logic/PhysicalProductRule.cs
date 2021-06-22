using BusinessRule.Model;
using BusinessRule.Service;

namespace BusinessRule.Logic
{
    public class PhysicalProductRule : IExecute<Payment<PhysicalProduct>>
    {
        private readonly IPackingSlipService _packingSlipService;
        private readonly IComissionPaymentService _comissionPaymentService;


        public PhysicalProductRule(IPackingSlipService packingSlipService, IComissionPaymentService comissionPaymentService)
        {
            _packingSlipService = packingSlipService;
            _comissionPaymentService = comissionPaymentService;
        }


        public void DoStuff(Payment<PhysicalProduct> message)
        {
            _packingSlipService.GeneratePackingSlip(message.Product);
            _comissionPaymentService.GenerateComissionPayment(message.Product);
        }
    }
}