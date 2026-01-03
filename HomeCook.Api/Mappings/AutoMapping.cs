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

            CreateMap<Food, FoodDTO>().ReverseMap();

            CreateMap<FoodImage, FoodImageDTO>().ReverseMap();

            CreateMap<AddUpdateFoodDTO, Food>().ReverseMap();

            CreateMap<Food, FoodDetailDTO>().ReverseMap();

            CreateMap<User, UserInfoDTO>().ReverseMap();

            CreateMap<Profile, UserProfileDTO>().ReverseMap();

            CreateMap<AddUpdateProfileDTO, Profile>().ReverseMap();

            CreateMap<Profile, AddUpdateProfileDTO>().ReverseMap();
        }
    }
}
