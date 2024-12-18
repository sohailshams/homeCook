using HomeCook.Api.DTOs;
using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetFoodListAsync();
        Task<Food?> GetFoodDetailAsync(Guid id);
        Task<Food> AddFoodAsync(Food food);
    }
}
