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
            var food = await dbContext.Foods.Include("Category").Include("FoodImages").Where(f => f.AvailableDate > DateTime.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59)).ToListAsync();
            return food;
        }

        public async Task<Food?> GetFoodDetailAsync(Guid foodId)
        {
            var foodDetail = await dbContext.Foods.Include("Category").Include("FoodImages").FirstOrDefaultAsync(f => f.Id == foodId);

            return foodDetail;
        }

        public async Task<List<Food>> GetFoodByCategoryIdAsync(Guid categoryId)
        {
            var food = await dbContext.Foods.Include("Category").Include("FoodImages").Where(f => f.CategoryId == categoryId && f.AvailableDate > DateTime.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59)).ToListAsync();
            return food;
        }

        public async Task<Food> AddFoodAsync(Food food)
        {

            food.AvailableDate = DateTime.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            await dbContext.Foods.AddAsync(food);
            await dbContext.SaveChangesAsync();
            return food;
        }

        public async Task<Food?> UpdateFoodAsync(Food updateFood)
        {
            dbContext.Update(updateFood);
            await dbContext.SaveChangesAsync();
            return updateFood;
        }

        public async Task<Food?> DeleteFoodByIdAsync(Guid foodId, string loggedInUserId)
        {
            var existingFood = await GetFoodDetailAsync(foodId);
            if (existingFood == null) return null;

            if (existingFood.SellerId.ToString() != loggedInUserId) { throw new UnauthorizedAccessException("You are not authorized to delete this food item."); }

            dbContext.Remove(existingFood);
            await dbContext.SaveChangesAsync();
            return existingFood;
        }
    }
}
