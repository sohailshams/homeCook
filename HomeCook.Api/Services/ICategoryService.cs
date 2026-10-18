using HomeCook.Api.Dtos;

namespace HomeCook.Api.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetCategoriesAsync();
        public Task<CategoryDto> AddCategoryAsync(AddUpdateCategoryDto addCategory);
        public Task<CategoryDto> UpdateCategoryByIdAsync(Guid categoryId, AddUpdateCategoryDto updateCategory);
        public Task<CategoryDto> DeleteCategoryByIdAsync(Guid categoryId);
    }
}