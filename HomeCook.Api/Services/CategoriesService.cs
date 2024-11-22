using HomeCook.Api.DTOs;
using HomeCook.Api.EntityFramework.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HomeCook.Api.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesReposity _categoriesReposity;
        public CategoriesService(ICategoriesReposity categoriesReposity)
        {
            _categoriesReposity = categoriesReposity;
        }
        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _categoriesReposity.GetCategoriesAsync();
            var categoriesDto = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDTO()
                {
                    Id = category.Id,
                    Name = category.Name,
                });
            }
            return categoriesDto;
        }
    }
}
