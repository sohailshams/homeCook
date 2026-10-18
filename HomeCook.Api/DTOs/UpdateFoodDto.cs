using System.ComponentModel.DataAnnotations;

namespace HomeCook.Api.DTOs
{
    public class UpdateFoodDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string[] Ingredients { get; set; } = [];
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        // 0 is allowed so a seller can mark the food as sold out
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int QuantityAvailable { get; set; }
        public DateTime AvailableDate { get; set; }
        public Guid CategoryId { get; set; }
        // Null or empty keeps the existing images
        public List<FoodImageDto>? FoodImages { get; set; }
    }
}
