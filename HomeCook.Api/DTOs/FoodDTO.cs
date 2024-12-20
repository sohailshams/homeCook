using HomeCook.Api.Models;

namespace HomeCook.Api.DTOs
{
    public class FoodDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime AvailableDate { get; set; }
        public int QuantityAvailable { get; set; }
        public required string SellerId { get; set; }
        public List<string>? FoodImageUrls { get; set; }
    }
}
