using System.Text.RegularExpressions;
using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.Services;

public class FoodSearchService : IFoodSearchService
{
    private readonly IFoodSearchRepository _foodSearchRepository;
    private readonly IMapper _mapper;

    public FoodSearchService(IFoodSearchRepository foodSearchRepository, IMapper mapper)
    {
        _foodSearchRepository = foodSearchRepository;
        _mapper = mapper;
    }
    public async Task<List<FoodDTO>> FoodSearchAsync(string foodSearchTerm)
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

            // map food list<Food> to food list<FoodDTO>
            if (foodList != null && foodList.Count > 0)
            {
                var foodDto = _mapper.Map<List<FoodDTO>>(foodList);
                return foodDto;
            }
            return new List<FoodDTO>();
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
