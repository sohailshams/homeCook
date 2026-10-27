using System;
using System.Linq;
using System.Text.RegularExpressions;
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
        // Keep only words made of letters/digits "chicken curry!" - ["chicken", "curry"]
        var words = Regex.Matches(foodSearchTerm ?? "", @"[\p{L}\p{N}]+")
            .Select(m => m.Value)
            .ToList();

        // Blank or symbols only input - nothing to search for
        if (words.Count == 0)
            return new List<Food>();

        // Build a valid tsquery - ["chicken", "curry"] - "chicken:* & curry:*"
        var tsQuery = string.Join(" & ", words.Select(w => w + ":*"));

        // Text for the category ILIKE, also from the clean words - "chicken curry"
        var categoryTerm = string.Join(" ", words);

        return await _dbContext.Foods
            .OpenForOrders()
            .Include(f => f.Category)
            .Include(f => f.FoodImages)
            .Where(f =>
                f.SearchVector.Matches(EF.Functions.ToTsQuery("english", tsQuery)) ||
                EF.Functions.ILike(f.Category.Name, $"%{categoryTerm}%"))
            .OrderByDescending(f => f.AvailableDate)
            .ToListAsync();
    }

    public async Task<List<Food>> FoodSearchPostCodeAsync(string foodSearchTerm)
    {
        if (string.IsNullOrWhiteSpace(foodSearchTerm))
            return new List<Food>();

        // PostgreSQL Search for postcode
        return await _dbContext.Foods
            .OpenForOrders()
            .Include("FoodImages")
            .Where(f => f.Seller.Addresses.Any(a => EF.Functions.ILike(a.PostCode, $"%{foodSearchTerm}%") && a.IsPrimary))
            .ToListAsync();
    }
}
