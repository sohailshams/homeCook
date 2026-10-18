using System.ComponentModel.DataAnnotations;

namespace HomeCook.Api.Dtos
{
    public class CreateFoodDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string[] Ingredients { get; set; } = [];
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int QuantityAvailable { get; set; }
        public DateTime AvailableDate { get; set; }
        public Guid CategoryId { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one food image is required.")]
        public List<FoodImageDto> FoodImages { get; set; }
    }
}
