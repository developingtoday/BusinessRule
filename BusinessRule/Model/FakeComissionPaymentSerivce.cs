using System.Collections.Generic;

namespace BusinessRule.Model
{
    public class FakeComissionPaymentSerivce:IComissionPaymentService
    {
        private readonly List<Payment<Commision>> _payments;
        public FakeComissionPaymentSerivce()
        {
            _payments = new List<Payment<Commision>>();
        }

        public void GenerateComissionPayment(Product product)
        {
            _payments.Add(new Payment<Commision>()
            {
                Amount = 100,
                Product = new Commision()
                {
                    Id = product.Id
                }
            });
        }

        public List<Payment<Commision>> GetComissionPayments()
        {
            return _payments;
        }
    }
}