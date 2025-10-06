using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services;

public interface IFoodSearchService
{
    Task<List<FoodDTO>> FoodSearchAsync(string foodSearchTerm);
}
