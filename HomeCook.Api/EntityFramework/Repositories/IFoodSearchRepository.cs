using System;
using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories;

public interface IFoodSearchRepository
{
    Task<List<Food>> FoodSearchAsync(string foodSearchTerm);
}
