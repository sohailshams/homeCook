﻿using AutoMapper;
using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Models;

namespace HomeCook.Api.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesReposity _categoriesReposity;
        private readonly IMapper _mapper;

        public CategoriesService(ICategoriesReposity categoriesReposity, IMapper mapper)
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
            // Convert AddCategoryDTO to model
            var categoryModel = _mapper.Map<Category>(addCategory);

            // Use model to create category in DB
            var newCategory = await _categoriesReposity.AddCategoryAsync(categoryModel);
            
            //Map model to DTO
            var categoryDto = _mapper.Map<CategoryDTO>(newCategory);

            return categoryDto;

        }
    }
}
