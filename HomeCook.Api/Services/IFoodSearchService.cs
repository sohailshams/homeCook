using HomeCook.Api.Dtos;

namespace HomeCook.Api.Services;

public interface IFoodSearchService
{
    Task<List<FoodDto>> FoodSearchAsync(string foodSearchTerm);
}
