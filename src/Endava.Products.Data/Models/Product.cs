using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endava.Products.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
    }
}
