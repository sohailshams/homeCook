using HomeCook.Api.DTOs;
using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetFoodListAsync();
        Task<Food?> GetFoodDetailAsync(Guid foodId);
        Task<Food> AddFoodAsync(Food food, List<string>? foodImageUrls);
        Task<Food?> DeleteFoodByIdAsync(Guid foodId, string loggedInUserId);
        Task<Food?> UpdateFoodAsync(Guid foodId, string loggedInUserId, Food updateFood);
    }
}
