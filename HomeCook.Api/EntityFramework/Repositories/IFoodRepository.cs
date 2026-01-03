using HomeCook.Api.DTOs;
using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetFoodListAsync();
        Task<Food?> GetFoodDetailAsync(Guid foodId);
        Task<List<Food>> GetFoodByCategoryIdAsync(Guid categoryId);
        Task<Food> AddFoodAsync(Food food);
        Task<Food?> DeleteFoodByIdAsync(Guid foodId, string loggedInUserId);
        Task<Food?> UpdateFoodAsync(Food updateFood);
    }
}
