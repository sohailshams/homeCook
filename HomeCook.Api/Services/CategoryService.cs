using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoryService(ICategoryReposity categoriesReposity, IMapper mapper)
        {
            _categoriesReposity = categoriesReposity;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            // get categories from db
            var categories = await _categoriesReposity.GetCategoriesAsync();

            // map categories<Category> to categories<CategoryDto>
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            return categoriesDto;
        }

        public async Task<CategoryDto> AddCategoryAsync(AddUpdateCategoryDto addCategory)
        {
            try
            {
            // Convert AddCategoryDto to model
            var categoryModel = _mapper.Map<Category>(addCategory);

            // Use model to create category in DB
            var newCategory = await _categoriesReposity.AddCategoryAsync(categoryModel) ??  throw new ValidationException("Category with same name already exist."); ;
            
            //Map model to DTO
            var categoryDto = _mapper.Map<CategoryDto>(newCategory);

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
                var categoryModel = _mapper.Map<Category>(updateCategory);
                var category = await _categoriesReposity.UpdateCategoryAsync(categoryId, categoryModel) ?? throw new ValidationException("Category not found.");

                //Map model to DTO
                var categoryDto = _mapper.Map<CategoryDto>(category);
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
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return categoryDto;

            } catch(DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to delete category.", exception);

            }

        }
    }
}
