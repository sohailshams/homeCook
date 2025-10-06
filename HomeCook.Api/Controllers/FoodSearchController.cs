using System.Linq;
using HomeCook.Api.DTOs;
using HomeCook.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodSearchController : ControllerBase
    {
        private readonly ILogger<FoodSearchController> _logger;
        private readonly IFoodSearchService _foodSearchService;

        public FoodSearchController(ILogger<FoodSearchController> logger, IFoodSearchService foodSearchService)
        {
            _logger = logger;
            _foodSearchService = foodSearchService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FoodSearchAsync([FromQuery] string foodSearchTerm)
        {
            var foodList = await _foodSearchService.FoodSearchAsync(foodSearchTerm);
            if (foodList.Count == 0)
                return NotFound("No matching foods found.");

            return Ok(foodList);
        }
    }
}
