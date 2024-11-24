using HomeCook.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoriesService categoriesService)
        {
            _logger = logger;
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoriesService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}