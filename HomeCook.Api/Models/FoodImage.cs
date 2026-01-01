namespace HomeCook.Api.Models
{
    public class FoodImage
    {
        public Guid Id { get; set; }
        public Guid FoodId { get; set; }
        public Food Food { get; set; } = null!;
        public required string ImageUrl { get; set; }
        public required string PublicId { get; set; }
    }
}
