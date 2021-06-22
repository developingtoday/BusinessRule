using BusinessRule.Model;
using BusinessRule.Service;

namespace BusinessRule.Logic
{
    public class BookProductRule : IExecute<Payment<BookProduct>>
    {
        private readonly IPackingSlipService _packingSlipService;
        private readonly IComissionPaymentService _comissionPaymentService;

        public BookProductRule(IPackingSlipService packingSlipService, IComissionPaymentService comissionPaymentService)
        {
            _packingSlipService = packingSlipService;
            _comissionPaymentService = comissionPaymentService;
        }

        public void DoStuff(Payment<BookProduct> payment)
        {
            _packingSlipService.DuplicatePackingSlip(payment.Product);
            _comissionPaymentService.GenerateComissionPayment(payment.Product);
        }
    }
}