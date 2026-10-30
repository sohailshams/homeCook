using HomeCook.Api.Dtos;
using HomeCook.Api.Models;

namespace HomeCook.Api.Mappings
{
    public static class FoodMappings
    {
        public static FoodDto ToDto(this Food food) => new()
        {
            Id = food.Id,
            AvailableDate = food.AvailableDate,
            OrderDeadline = food.OrderDeadline,
            Name = food.Name,
            Price = food.Price,
            Description = food.Description,
            Ingredients = food.Ingredients,
            QuantityAvailable = food.QuantityAvailable,
            SellerId = food.SellerId,
            FoodImages = food.FoodImages?.Select(image => image.ToDto()).ToList() ?? [],
            CategoryId = food.CategoryId
        };

        public static FoodDetailDto ToDetailDto(this Food food) => new()
        {
            Id = food.Id,
            Name = food.Name,
            Description = food.Description,
            Price = food.Price,
            AvailableDate = food.AvailableDate,
            OrderDeadline = food.OrderDeadline,
            QuantityAvailable = food.QuantityAvailable,
            Ingredients = food.Ingredients,
            SellerId = food.SellerId,
            FoodImages = food.FoodImages?.Select(image => image.ToDto()).ToList() ?? [],
            Category = food.Category.ToDto(),
            CategoryId = food.CategoryId
        };

        public static Food ToModel(this CreateFoodDto dto, Guid sellerId) => new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Ingredients = dto.Ingredients.ToList(),
            Price = dto.Price,
            QuantityAvailable = dto.QuantityAvailable,
            // [Required] on the DTO guarantees both dates are present
            AvailableDate = dto.AvailableDate!.Value,
            OrderDeadline = dto.OrderDeadline!.Value,
            CategoryId = dto.CategoryId,
            SellerId = sellerId,
            FoodImages = dto.FoodImages.Select(image => image.ToModel()).ToList()
        };

        public static FoodImageDto ToDto(this FoodImage image) => new()
        {
            ImageUrl = image.ImageUrl,
            PublicId = image.PublicId
        };

        public static FoodImage ToModel(this FoodImageDto dto) => new()
        {
            ImageUrl = dto.ImageUrl,
            PublicId = dto.PublicId
        };
    }
}
