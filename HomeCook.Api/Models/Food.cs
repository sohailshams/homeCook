using NpgsqlTypes;

namespace HomeCook.Api.Models
{
    public class Food
    {
        public Guid Id { get; set; }
        public DateTime AvailableDate { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public required List<string> Ingredients { get; set; }
        public int QuantityAvailable { get; set; }
        public required Guid SellerId { get; set; }
        public User Seller { get; set; } = null!;
        public List<FoodImage>? FoodImage { get; set; }
        public required Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Added for full-text search
        public NpgsqlTsVector SearchVector { get; set; } = null!;

    }
}
