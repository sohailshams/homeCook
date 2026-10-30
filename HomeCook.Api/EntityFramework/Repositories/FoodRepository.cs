using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly AppDbContext dbContext;

        public FoodRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Food>> GetFoodListAsync(Point location, double? radius)
        {
            var baseQuery = dbContext.Foods.OpenForOrders();

            if (location != null && radius != null)
            {
                double meters = radius.Value * 1609.34;

                // Find seller ids within radius, ordered by distance, pushed to SQL
                var matches = await dbContext.Addresses
                    .Where(a => a.IsPrimary && a.Location.IsWithinDistance(location, meters))
                    .Join(baseQuery,
                          a => a.UserId,
                          f => f.SellerId,
                          (a, f) => new { f.Id, Distance = a.Location.Distance(location) })
                    .OrderBy(x => x.Distance)
                    .ToListAsync();

                var orderedIds = matches.Select(x => x.Id).ToList();

                var foods = await dbContext.Foods
                    .Include(f => f.Category)
                    .Include(f => f.FoodImages)
                    .Where(f => orderedIds.Contains(f.Id))
                    .ToListAsync();

                // preserve distance ordering
                return foods.OrderBy(f => orderedIds.IndexOf(f.Id)).ToList();
            }

            return await baseQuery
                .Include(f => f.Category)
                .Include(f => f.FoodImages)
                .ToListAsync();
        }

        public async Task<Food?> GetFoodDetailAsync(Guid foodId)
        {
            var foodDetail = await dbContext.Foods.Include("Category").Include("FoodImages").FirstOrDefaultAsync(f => f.Id == foodId);

            return foodDetail;
        }

        public async Task<List<Food>> GetFoodByCategoryIdAsync(Guid categoryId)
        {
            var food = await dbContext.Foods.Include("Category").Include("FoodImages").OpenForOrders().Where(f => f.CategoryId == categoryId).ToListAsync();
            return food;
        }

        public async Task<Food> AddFoodAsync(Food food)
        {

            await dbContext.Foods.AddAsync(food);
            await dbContext.SaveChangesAsync();
            return food;
        }

        public async Task<Food?> UpdateFoodAsync(Food updateFood)
        {
            // updateFood was loaded by this DbContext, so EF already tracks it and saves only what changed
            await dbContext.SaveChangesAsync();
            return updateFood;
        }

        public async Task<Food> DeleteFoodAsync(Food food)
        {
            dbContext.Remove(food);
            await dbContext.SaveChangesAsync();
            return food;
        }
    }
}
