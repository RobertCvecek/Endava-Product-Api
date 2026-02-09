using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Endava.Products.Data.Requests
{
    public class UpdateProductRequest
    {
        [MinLength(2)]
        public string? Name { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
