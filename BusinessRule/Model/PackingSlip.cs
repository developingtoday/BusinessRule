using System;
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
}
