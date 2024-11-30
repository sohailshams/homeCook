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
        public async Task<IActionResult> AddCategory([FromBody] AddUpdateCategoryDTO addCategory)
        {
            var newCategory = await _categoriesService.AddCategoryAsync(addCategory);

            return Ok(newCategory);
        }


        [HttpPut]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] AddUpdateCategoryDTO updatedCategory)
        {
            var category = await _categoriesService.UpdateCategoryByIdAsync(categoryId, updatedCategory);

            return Ok(new
            {
                Message = "Category updated successfully",
                Category = category
            });
        }

        [HttpDelete]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            var category = await _categoriesService.DeleteCategoryByIdAsync(categoryId);

            return Ok(new
            {
                Message = "Category deleted successfully",
                Category = category
            });
        }
    }
}