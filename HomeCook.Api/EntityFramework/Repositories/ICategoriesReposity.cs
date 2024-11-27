using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface ICategoriesReposity
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<Category> AddCategoryAsync(Category category);
    }
}