using HomeCook.Api.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public class UpdateQuantityRepository: IUpdateQuantityRepository
    {
        private readonly AppDbContext _dbContext;
        public UpdateQuantityRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateItemDTO?> UpdateQuantityAsync(Guid foodId, int quantity)
        {
            var foodItem = await _dbContext.Foods.FirstOrDefaultAsync(f => f.Id == foodId);
            if (foodItem == null) return null;
            foodItem.QuantityAvailable -= quantity;
            await _dbContext.SaveChangesAsync();

            return new UpdateItemDTO
            {
                FoodId = foodItem.Id,
                Quantity = foodItem.QuantityAvailable
            };
        }
    }
}
