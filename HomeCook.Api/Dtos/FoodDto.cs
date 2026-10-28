using HomeCook.Api.Models;

namespace HomeCook.Api.Dtos
{
    public class FoodDto
    {
        public Guid Id { get; set; }
        public DateTime AvailableDate { get; set; }
        public DateTime OrderDeadline { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public required List<string> Ingredients { get; set; }
        public int QuantityAvailable { get; set; }
        public required Guid SellerId { get; set; }
        public List<FoodImageDto> FoodImages { get; set; }
        public required Guid CategoryId { get; set; }
    }
}
