using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface ICategoryReposity
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<Category?> AddCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(Guid categoryId, Category updateCategory);
        Task<Category?> DeleteCategoryAsync(Guid categoryId);
    }
}