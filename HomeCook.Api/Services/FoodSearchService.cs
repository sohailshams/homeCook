using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
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

            var foodList = await _foodSearchRepository.FoodSearchAsync(foodSearchTerm);

            // map food list<Food> to food list<FoodDTO>
            if (foodList.Count > 0)
            {
                var foodDto = _mapper.Map<List<FoodDTO>>(foodList);
                return foodDto;
            }
            return [];
        }
        catch (DbUpdateException exception)
        {
            throw new DatabaseOperationException("Failed to create food.", exception);
        }
    }
}
