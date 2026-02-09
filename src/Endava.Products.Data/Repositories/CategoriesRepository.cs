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
    public class CategoriesRepository(ProductsDbContext db) : ICategoriesRepository
    {
        public Task<Category?> GetById(Guid id)
        {
            return db.Catageories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
