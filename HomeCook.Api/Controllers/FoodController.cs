using HomeCook.Api.DTOs;
using HomeCook.Api.Models;
using HomeCook.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly ILogger<FoodController> _logger;
        private readonly IFoodService _foodService;

        public FoodController(ILogger<FoodController> logger, IFoodService foodService)
        {
            _logger = logger;
            _foodService = foodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoodList()
        {
            var food = await _foodService.GetFoodListAsync();
            return Ok(food);
        }

        [HttpGet]
        [Route("{foodId:Guid}")]
        public async Task<IActionResult> GetFoodDetail([FromRoute] Guid foodId)
        {
            var foodDetail = await _foodService.GetFoodDetailAsync(foodId);

            return Ok(foodDetail);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFood([FromBody] AddFoodDTO addFood)
        {
             var newFood = await _foodService.AddFoodAsync(addFood);
            return Ok(newFood);
        }

        [HttpDelete]
        [Route("{foodId:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteFood([FromRoute] Guid foodId)
        {
            var food = await _foodService.DeleteFoodByIdAsync(foodId);
            return Ok(new {
                Message = "Food deleted successfully",
                Food = food
            });
        }
    }
}
