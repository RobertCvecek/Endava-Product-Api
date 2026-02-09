using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Endava.Products.Data.Dtos;
using Endava.Products.Data.Requests;

namespace Endava.Products.Data.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<ProductDto>> Fetch(ProductsFilterRequest filter);
    }
}
