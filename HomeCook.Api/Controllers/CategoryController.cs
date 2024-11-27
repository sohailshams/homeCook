using HomeCook.Api.DTOs;
using HomeCook.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoriesService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoriesService)
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

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO addCategory)
        {
            var newCategory = await _categoriesService.AddCategoryAsync(addCategory);

            return Ok(newCategory);
        }
    }
}