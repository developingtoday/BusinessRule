using System.Collections.Generic;
using BusinessRule.Model;

namespace BusinessRule.Service
{
    public interface IComissionPaymentService
    {
        void GenerateComissionPayment(Product product);

        List<Payment<Commision>> GetComissionPayments();

    }
}