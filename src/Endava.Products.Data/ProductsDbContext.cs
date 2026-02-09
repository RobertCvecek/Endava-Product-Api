using Endava.Products.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Endava.Products.Data
{
    public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Catageories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();
    }
}
