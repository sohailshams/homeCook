using System.Text.RegularExpressions;
using HomeCook.Api.Mappings;
using HomeCook.Api.Dtos;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.Services;

public class FoodSearchService : IFoodSearchService
{
    private readonly IFoodSearchRepository _foodSearchRepository;

    public FoodSearchService(IFoodSearchRepository foodSearchRepository)
    {
        _foodSearchRepository = foodSearchRepository;
    }
    public async Task<List<FoodDto>> FoodSearchAsync(string foodSearchTerm)
    {
        try
        {
            List<Food> foodList;
            foodSearchTerm = foodSearchTerm.Trim();

            if (IsPostcode(foodSearchTerm))
            {
                foodList = await _foodSearchRepository.FoodSearchPostCodeAsync(foodSearchTerm);
            }
            else
            {
                foodList = await _foodSearchRepository.FoodSearchAsync(foodSearchTerm);
            }

            // map food list<Food> to food list<FoodDto>
            if (foodList != null && foodList.Count > 0)
            {
                var foodDto = foodList.Select(f => f.ToDto()).ToList();
                return foodDto;
            }
            return new List<FoodDto>();
        }
        catch (DbUpdateException exception)
        {
            throw new DatabaseOperationException("Failed to load food.", exception);
        }
    }

    private bool IsPostcode(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        var pattern = @"^[A-Z]{1,2}\d[A-Z\d]?\s*\d[A-Z]{2}$";

        return Regex.IsMatch(input.Trim(), pattern, RegexOptions.IgnoreCase);
    }
}
