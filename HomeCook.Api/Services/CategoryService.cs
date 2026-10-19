using HomeCook.Api.Mappings;
using HomeCook.Api.Dtos;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HomeCook.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryReposity _categoriesReposity;

        public CategoryService(ICategoryReposity categoriesReposity)
        {
            _categoriesReposity = categoriesReposity;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            // get categories from db
            var categories = await _categoriesReposity.GetCategoriesAsync();

            // map categories<Category> to categories<CategoryDto>
            var categoriesDto = categories.Select(category => category.ToDto()).ToList();
            return categoriesDto;
        }

        public async Task<CategoryDto> AddCategoryAsync(AddUpdateCategoryDto addCategory)
        {
            try
            {
            // Convert AddCategoryDto to model
            var categoryModel = addCategory.ToModel();

            // Use model to create category in DB
            var newCategory = await _categoriesReposity.AddCategoryAsync(categoryModel) ??  throw new ValidationException("Category with same name already exist."); ;
            
            //Map model to DTO
            var categoryDto = newCategory.ToDto();

            return categoryDto;

            } catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to create category.", exception);
            }

        }


        public async Task<CategoryDto> UpdateCategoryByIdAsync(Guid categoryId, AddUpdateCategoryDto updateCategory)
        {
            try
            {
                // Convert AddCategoryDto to model
                var categoryModel = updateCategory.ToModel();
                var category = await _categoriesReposity.UpdateCategoryAsync(categoryId, categoryModel) ?? throw new ValidationException("Category not found.");

                //Map model to DTO
                var categoryDto = category.ToDto();
                return categoryDto;

            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to update category.", exception);

            }
        }

        public async Task<CategoryDto> DeleteCategoryByIdAsync(Guid categoryId)
        {
            try
            {
                var category = await _categoriesReposity.DeleteCategoryAsync(categoryId) ?? throw new NotFoundException("Category not found.");

                //Map model to DTO
                var categoryDto = category.ToDto();
                return categoryDto;

            } catch(DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to delete category.", exception);

            }

        }
    }
}
