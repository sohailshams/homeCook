using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryDTO>> GetCategoriesAsync();
        public Task<CategoryDTO> AddCategoryAsync(AddCategoryDTO addCategory);
    }
}