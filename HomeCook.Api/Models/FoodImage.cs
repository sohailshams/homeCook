namespace HomeCook.Api.Models
{
    public class FoodImage
    {
        public Guid Id { get; set; }
        public Guid FoodId { get; set; }  
        public Food Food { get; set; }
        public required string Image { get; set; }
    }
}
