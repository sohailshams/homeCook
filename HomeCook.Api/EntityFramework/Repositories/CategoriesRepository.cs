using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public class CategoriesRepository : ICategoriesReposity
    {
        private readonly AppDbContext dbContext;
        public CategoriesRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await dbContext.Categories.ToListAsync();
            return categories;
        }
    }
}