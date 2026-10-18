using HomeCook.Api.Dtos;

namespace HomeCook.Api.Services
{
    public interface IFoodService
    {
        public Task<List<FoodDto>> GetFoodListAsync(string postCode, double? radius);
        public Task<FoodDetailDto?> GetFoodDetailAsync(Guid foodId);
        public Task<List<FoodDto>> GetFoodByCategoryIdAsync(Guid categoryId);
        public Task<FoodDto> AddFoodAsync(CreateFoodDto addFoodDto);
        public Task<FoodDto> UpdateFoodAsync(Guid foodId, UpdateFoodDto updateFood);
        public Task<FoodDto> DeleteFoodByIdAsync(Guid foodId);
    }
}
