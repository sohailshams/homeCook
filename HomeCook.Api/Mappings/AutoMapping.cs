using HomeCook.Api.Dtos;
using HomeCook.Api.Models;

namespace HomeCook.Api.Mappings
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<AddUpdateCategoryDto, Category>().ReverseMap();

            CreateMap<Food, FoodDto>().ReverseMap();

            CreateMap<FoodImage, FoodImageDto>().ReverseMap();

            CreateMap<CreateFoodDto, Food>();

            CreateMap<UpdateFoodDto, Food>();

            CreateMap<Food, FoodDetailDto>().ReverseMap();

            CreateMap<User, UserInfoDto>().ReverseMap();

            CreateMap<Profile, UserProfileDto>().ReverseMap();

            CreateMap<AddUpdateProfileDto, Profile>().ReverseMap();

            CreateMap<Profile, AddUpdateProfileDto>().ReverseMap();
        }
    }
}
