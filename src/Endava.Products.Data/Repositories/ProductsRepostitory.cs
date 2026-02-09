using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Endava.Products.Data.Dtos;
using Endava.Products.Data.Interfaces;
using Endava.Products.Data.Models;
using Endava.Products.Data.Requests;
using Microsoft.EntityFrameworkCore;

namespace Endava.Products.Data.Repositories
{
    public class ProductsRepostitory(ProductsDbContext db) : IProductsRepository
    {
        public async Task<IEnumerable<ProductDto>> Fetch(ProductsFilterRequest filter)
        {
            IQueryable<Product> query = db.Products.Include(p => p.Category);

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                var name = filter.Name.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(name));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            return await query
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Category = new CategoryDto
                    {
                        Name = p.Category.Name,
                        Description = p.Category.Description,
                    },
                })
                .ToListAsync();
        }
    }
}
