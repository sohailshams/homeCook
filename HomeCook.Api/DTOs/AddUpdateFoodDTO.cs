namespace HomeCook.Api.DTOs
{
    public class AddUpdateFoodDTO
    {
        public string Name { get; set; } 
        public string Description { get; set; } 
        public string Ingredients { get; set; } 
        public decimal Price { get; set; } 
        public int QuantityAvailable { get; set; } 
        public DateTime AvailableDate { get; set; } 
        public Guid CategoryId { get; set; }
        public List<string>? FoodImageUrls { get; set; } 
        public string SellerId { get; set; }

    }
}
