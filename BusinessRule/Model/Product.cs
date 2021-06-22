using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessRule.Model
{
    public abstract class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
