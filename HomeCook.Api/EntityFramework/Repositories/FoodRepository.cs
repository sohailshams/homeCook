using HomeCook.Api.DTOs;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
            var foodDetail = await dbContext.Foods.Include("Category").Include("FoodImage").FirstOrDefaultAsync(f => f.Id == foodId);
            
            return foodDetail;
        }

        public async Task<Food> AddFoodAsync(Food food)
        {
            await dbContext.Foods.AddAsync(food);
            await dbContext.SaveChangesAsync();
            return food;
        }

        public async Task<Food?> UpdateFoodAsync(Guid foodId, string loggedInUserId, Food updateFood)
        {           
            var existingFood = await GetFoodDetailAsync(foodId);
            if (existingFood == null) return null;

            if (existingFood.SellerId != loggedInUserId) { throw new UnauthorizedAccessException("You are not authorized to update this food item."); }
            existingFood.Name = updateFood.Name;
            existingFood.Description = updateFood.Description;
            existingFood.AvailableDate = updateFood.AvailableDate;
            existingFood.QuantityAvailable = updateFood.QuantityAvailable;
            existingFood.Price = updateFood.Price;
            existingFood.CategoryId = updateFood.CategoryId;
            existingFood.Ingredients = updateFood.Ingredients;
            existingFood.SellerId = updateFood.SellerId;
            existingFood.FoodImage = updateFood.FoodImage;

            await dbContext.SaveChangesAsync();
            return existingFood;
        }

        public async Task<Food?> DeleteFoodByIdAsync(Guid foodId, string loggedInUserId)
        {
            var existingFood = await GetFoodDetailAsync(foodId);
            if (existingFood == null) return null;

            if (existingFood.SellerId != loggedInUserId) { throw new UnauthorizedAccessException("You are not authorized to delete this food item."); }

            dbContext.Remove(existingFood);
            await dbContext.SaveChangesAsync();
            return existingFood;
        }
    }
}
