using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface ICategoriesService
    {
        public Task<List<CategoryDTO>> GetCategoriesAsync();
        public Task<CategoryDTO> AddCategoryAsync(AddCategoryDTO addCategory);
    }
}