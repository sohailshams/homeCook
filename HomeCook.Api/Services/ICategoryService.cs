using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryDTO>> GetCategoriesAsync();
        public Task<CategoryDTO> AddCategoryAsync(AddCategoryDTO addCategory);
        public Task<CategoryDTO> UpdateCategoryByIdAsync(Guid categoryId, AddCategoryDTO updateCategory);
        public Task<CategoryDTO> DeleteCategoryByIdAsync(Guid categoryId);
    }
}