namespace HomeCook.Api.DTOs
{
    public class AddFoodDTO
    {
        public string Name { get; set; } 
        public string Description { get; set; } 
        public string Ingredients { get; set; } 
        public decimal Price { get; set; } 
        public int QuantityAvailable { get; set; } 
        public DateTime AvailableDate { get; set; } 
        public Guid CategoryId { get; set; }
        public Guid? FoodImageId { get; set; }
        public List<string>? FoodImageUrls { get; set; } 
        public string SellerId { get; set; }

    }
}
