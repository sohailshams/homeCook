using AutoMapper;
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
    }
}
