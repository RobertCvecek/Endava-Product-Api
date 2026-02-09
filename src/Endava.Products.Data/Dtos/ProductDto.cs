using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endava.Products.Data.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public CategoryDto Category { get; set; } = null!;
    }
}
