using AutoMapper;
using HomeCook.Api.DTOs;
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

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            // get categories from db
            var categories = await _categoriesReposity.GetCategoriesAsync();

            // map categories<Category> to categories<CategoryDTO>
            var categoriesDto = _mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDto;
        }

        public async Task<CategoryDTO> AddCategoryAsync(AddCategoryDTO addCategory)
        {
            try
            {
            // Convert AddCategoryDTO to model
            var categoryModel = _mapper.Map<Category>(addCategory);

            // Use model to create category in DB
            var newCategory = await _categoriesReposity.AddCategoryAsync(categoryModel) ??  throw new ValidationException("Category with same name already exist."); ;
            
            //Map model to DTO
            var categoryDto = _mapper.Map<CategoryDTO>(newCategory);

            return categoryDto;

            } catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to create category.", exception);
            }

        }


        public async Task<CategoryDTO> UpdateCategoryByIdAsync(Guid categoryId, AddCategoryDTO updateCategory)
        {
            try
            {
                // Convert AddCategoryDTO to model
                var categoryModel = _mapper.Map<Category>(updateCategory);
                var category = await _categoriesReposity.UpdateCategoryAsync(categoryId, categoryModel) ?? throw new ValidationException("Category not found.");

                //Map model to DTO
                var categoryDto = _mapper.Map<CategoryDTO>(category);
                return categoryDto;

            }
            catch (DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to update category.", exception);

            }
        }

        public async Task<CategoryDTO> DeleteCategoryByIdAsync(Guid categoryId)
        {
            try
            {
                var category = await _categoriesReposity.DeleteCategoryAsync(categoryId) ?? throw new ValidationException("Category not found.");

                //Map model to DTO
                var categoryDto = _mapper.Map<CategoryDTO>(category);
                return categoryDto;

            } catch(DbUpdateException exception)
            {
                throw new DatabaseOperationException("Failed to delete category.", exception);

            }

        }
    }
}
