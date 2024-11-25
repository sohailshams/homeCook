using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Category> AddCategoryAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

    }
}