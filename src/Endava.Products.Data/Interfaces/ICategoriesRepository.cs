using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Endava.Products.Data.Models;

namespace Endava.Products.Data.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<Category?> GetById(Guid id);
    }
}
