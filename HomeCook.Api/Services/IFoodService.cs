using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface IFoodService
    {
        public Task<List<FoodDTO>> GetFoodListAsync();
    }
}
