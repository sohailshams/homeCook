using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryDTO>> GetCategoriesAsync();
        public Task<CategoryDTO> AddCategoryAsync(AddUpdateCategoryDTO addCategory);
        public Task<CategoryDTO> UpdateCategoryByIdAsync(Guid categoryId, AddUpdateCategoryDTO updateCategory);
        public Task<CategoryDTO> DeleteCategoryByIdAsync(Guid categoryId);
    }
}