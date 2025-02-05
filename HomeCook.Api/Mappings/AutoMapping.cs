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
            CreateMap<AddUpdateFoodDTO, Food>()
                .ForMember(dest => dest.FoodImage, opt => opt.MapFrom((src, dest) => src.FoodImageUrls != null
                ? src.FoodImageUrls.Select(url => new FoodImage { Image = url, Food = dest }).ToList()
                : new List<FoodImage>()));
            CreateMap<Food, FoodDetailDTO>()
                .ForMember(dest => dest.FoodImageUrls, opt => opt.MapFrom(src => src.FoodImage != null
                ? src.FoodImage.Select(img => img.Image).ToList()
                : new List<string>()));
            CreateMap<User, UserInfoDTO>().ReverseMap();
            CreateMap<Profile, UserProfileDTO>().ReverseMap();
            CreateMap<AddUpdateProfileDTO, Profile>().ReverseMap();
        }
    }
}
