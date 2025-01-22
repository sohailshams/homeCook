using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IFoodService
    {
        public Task<List<FoodDTO>> GetFoodListAsync();
        public Task<FoodDetailDTO?> GetFoodDetailAsync(Guid foodId);
        public Task<List<FoodDTO>> GetFoodByCategoryIdAsync(Guid categoryId);
        public Task<FoodDTO> AddFoodAsync(AddUpdateFoodDTO addFoodDTO);
        public Task<FoodDTO> UpdateFoodAsync(Guid foodId, AddUpdateFoodDTO updateFood);
        public Task<FoodDTO> DeleteFoodByIdAsync(Guid foodId);
    }
}
