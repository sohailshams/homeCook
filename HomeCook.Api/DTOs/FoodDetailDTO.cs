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
        public required List<string> Ingredients { get; set; }
        public required Guid SellerId { get; set; }
        public List<FoodImage> FoodImages { get; set; }
        public required Category Category { get; set; }
        public required Guid CategoryId { get; set; }
        public required string PostCode { get; set; }
    }
}
