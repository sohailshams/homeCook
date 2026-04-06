using System;
using System.Linq;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.EntityFramework.Repositories;

public class FoodSearchRepositoy : IFoodSearchRepository
{
    private readonly AppDbContext _dbContext;

    public FoodSearchRepositoy(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Food>> FoodSearchAsync(string foodSearchTerm)
    {
        if (string.IsNullOrWhiteSpace(foodSearchTerm))
            return new List<Food>();

        // PostgreSQL Full Text Search with EF.Functions
        return await _dbContext.Foods
            .Include(f => f.Category)
            .Include("FoodImages")
            .Where(f =>
                f.SearchVector.Matches(
                    EF.Functions.ToTsQuery("english", $"{foodSearchTerm}:*")) ||
                    EF.Functions.ILike(f.Category.Name, $"%{foodSearchTerm}%"))
                .OrderByDescending(f => f.AvailableDate)
                .ToListAsync();
    }

    public async Task<List<Food>> FoodSearchPostCodeAsync(string foodSearchTerm)
    {
        if (string.IsNullOrWhiteSpace(foodSearchTerm))
            return new List<Food>();

        // PostgreSQL Search for postcode
        return await _dbContext.Foods
            .Include("FoodImages")
            .Where(f => f.Seller.Addresses.Any(a => EF.Functions.ILike(a.PostCode, $"%{foodSearchTerm}%") && a.IsPrimary))
            .ToListAsync();
    }
}
