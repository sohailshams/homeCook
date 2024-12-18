using HomeCook.Api.DTOs;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly AppDbContext dbContext;

        public FoodRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Food>> GetFoodListAsync()
        {
            var food = await dbContext.Foods.Include("Category").ToListAsync();
            return food;
        }


        public async Task<Food?> GetFoodDetailAsync(Guid foodId)
        {
            var foodDetail = await dbContext.Foods.Include("Category").FirstOrDefaultAsync(f => f.Id == foodId);
            
            return foodDetail;
        }

        public async Task<Food> AddFoodAsync(Food food)
        {
            await dbContext.Foods.AddAsync(food);
            await dbContext.SaveChangesAsync();
            return food;
        }

        public async Task<Food?> DeleteFoodByIdAsync(Guid foodId, string sellerId)
        {
            var food = await GetFoodDetailAsync(foodId);
            if (food == null) return null;

            if (food.SellerId != sellerId) { throw new UnauthorizedAccessException("You are not authorized to delete this food item."); }

            dbContext.Remove(food);
            await dbContext.SaveChangesAsync();
            return food;
        }
    }
}
