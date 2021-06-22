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
}
