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

    public class PhysicalProduct : Product
    {

    }

    public class BookProduct:Product
    {
        
    }

    public class Payment<T> where T:Product
    {
        public T Product { get; set; }

        public decimal Amount { get; set; }

    }
}
