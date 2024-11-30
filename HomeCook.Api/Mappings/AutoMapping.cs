using HomeCook.Api.DTOs;
using HomeCook.Api.Models;

namespace HomeCook.Api.Mappings
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<AddUpdateCategoryDTO, Category>().ReverseMap();
        }
    }
}
