namespace HomeCook.Api.Models
{
    public class FoodImage
    {
        public Guid Id { get; set; }
        public required Guid FoodId { get; set; }
        public Food Food { get; set; } = null!;
        public required string Image { get; set; }
    }
}
