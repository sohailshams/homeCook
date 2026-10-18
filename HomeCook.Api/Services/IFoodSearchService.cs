using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services;

public interface IFoodSearchService
{
    Task<List<FoodDto>> FoodSearchAsync(string foodSearchTerm);
}
