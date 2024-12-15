using HomeCook.Api.Services;
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
    }
}
