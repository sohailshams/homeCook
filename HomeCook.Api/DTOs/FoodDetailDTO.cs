using HomeCook.Api.Models;

namespace HomeCook.Api.DTOs
{
    public class FoodDetailDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime AvailableDate { get; set; }
        public int QuantityAvailable { get; set; }
        public required string Ingredients { get; set; }
        public required string SellerId { get; set; }
        public Guid FoodImageId { get; set; }
        public List<FoodImage>? FoodImage { get; set; }
        public required Category Category { get; set; }
    }
}
