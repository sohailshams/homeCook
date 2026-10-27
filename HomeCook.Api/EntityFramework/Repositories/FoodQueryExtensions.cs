using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public static class FoodQueryExtensions
    {
        // Only food whose order deadline has not passed can still be bought
        public static IQueryable<Food> OpenForOrders(this IQueryable<Food> foods)
        {
            var now = DateTime.UtcNow;
            return foods.Where(f => f.OrderDeadline > now);
        }
    }
}
