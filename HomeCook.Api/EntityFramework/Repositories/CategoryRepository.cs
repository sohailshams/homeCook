using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public class CategoryRepository : ICategoryReposity
    {
        private readonly AppDbContext dbContext;
        public CategoryRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await dbContext.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            return existingCategory;
        }

        public async Task<Category?> AddCategoryAsync(Category category)
        {
            var isExistingCategory = await dbContext.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());
            if (isExistingCategory) return null;

            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(Guid categoryId, Category upateCategory)
        {
            var existingCategory = await GetCategoryByIdAsync(categoryId);
            if (existingCategory == null) return null;

            existingCategory.Name = upateCategory.Name;
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<Category?> DeleteCategoryAsync(Guid categoryId)
        {
            var category = await GetCategoryByIdAsync(categoryId);
            if (category == null) return null;

            dbContext.Remove(category);
            await dbContext.SaveChangesAsync();
            return category;
        }
    }
}