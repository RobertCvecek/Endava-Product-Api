using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endava.Products.Data.Requests
{
    public class ProductsFilterRequest
    {
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
