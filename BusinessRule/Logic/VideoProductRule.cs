using System;
using BusinessRule.Model;
using BusinessRule.Service;

namespace BusinessRule.Logic
{
    public class VideoProductRule:IExecute<Payment<VideoProduct>>
    {
        private readonly IPackingSlipService _packingSlipService;

        public VideoProductRule(IPackingSlipService packingSlipService)
        {
            _packingSlipService = packingSlipService;
        }

        public void DoStuff(Payment<VideoProduct> message)
        {
            _packingSlipService.GeneratePackingSlip(message.Product);
            if (message.Product.Name.Equals("Learning to Ski", StringComparison.InvariantCultureIgnoreCase))
            {
                _packingSlipService.GeneratePackingSlip(new VideoProduct()
                {
                    Id = Guid.NewGuid(),
                    Name = "First Aid"
                });
            }
        }
    }
}