using System.Collections.Generic;

namespace BusinessRule.Model
{
    public interface IComissionPaymentService
    {
        void GenerateComissionPayment(Product product);

        List<Payment<Commision>> GetComissionPayments();

    }
}