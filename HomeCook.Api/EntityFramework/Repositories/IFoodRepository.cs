using HomeCook.Api.Dtos;
using HomeCook.Api.Models;
using NetTopologySuite.Geometries;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IFoodRepository
    {
        Task<List<Food>> GetFoodListAsync(Point location, double? radius);
        Task<Food?> GetFoodDetailAsync(Guid foodId);
        Task<List<Food>> GetFoodByCategoryIdAsync(Guid categoryId);
        Task<Food> AddFoodAsync(Food food);
        Task<Food> DeleteFoodAsync(Food food);
        Task<Food?> UpdateFoodAsync(Food updateFood);
    }
}
