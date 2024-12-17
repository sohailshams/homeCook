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
            var food = await dbContext.Foods.ToListAsync();
            return food;
        }

        public async Task<Food> AddFoodAsync(Food food)
        {
            await dbContext.Foods.AddAsync(food);
            await dbContext.SaveChangesAsync();
            return food;
        }
    }
}
