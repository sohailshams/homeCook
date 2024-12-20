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
            CreateMap<Food, FoodDTO>()
            .ForMember(dest => dest.FoodImageUrls, opt => opt.MapFrom(src => src.FoodImage != null
             ? src.FoodImage.Select(img => img.Image).ToList()
             : new List<string>()));
            CreateMap<AddUpdateFoodDTO, Food>().ReverseMap();
            CreateMap<FoodDetailDTO, Food>().ReverseMap();
            CreateMap<AddUpdateFoodDTO, FoodDTO>();


        }
    }
}
