using HomeCook.Api.Dtos;
using HomeCook.Api.Models;

namespace HomeCook.Api.Mappings
{
    public static class CategoryMappings
    {
        public static CategoryDto ToDto(this Category category) => new()
        {
            Id = category.Id,
            Name = category.Name
        };

        public static Category ToModel(this AddUpdateCategoryDto dto) => new()
        {
            Name = dto.Name
        };
    }
}
