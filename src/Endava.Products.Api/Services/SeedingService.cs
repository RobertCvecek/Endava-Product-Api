using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Endava.Products.Data;
using Endava.Products.Data.Models;

namespace Endava.Products.Api.Services
{
    public class SeedingService(ProductsDbContext dbContext)
    {
        public async Task Seed()
        {
            Category[] categories =
            [
                new Category { Name = "Category 1", Description = "Category 1 description" },
                new Category { Name = "Category 2", Description = "Category 2 description" },
            ];
            if (!dbContext.Catageories.Any())
            {
                await dbContext.Catageories.AddRangeAsync(categories);
            }
            if (!dbContext.Products.Any())
            {
                await dbContext.Products.AddRangeAsync([
                    new Product
                    {
                        Name = "Product 1",
                        Price = 2.42m,
                        CategoryId = categories[0].Id,
                    },
                    new Product
                    {
                        Name = "Product 2",
                        Price = 54.12m,
                        CategoryId = categories[1].Id,
                    },
                ]);
            }
            dbContext.SaveChanges();
        }
    }
}
