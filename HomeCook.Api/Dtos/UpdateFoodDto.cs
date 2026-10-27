using System.ComponentModel.DataAnnotations;

namespace HomeCook.Api.Dtos
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
        [Required(ErrorMessage = "Available date is required.")]
        public DateTime? AvailableDate { get; set; }
        [Required(ErrorMessage = "Order deadline is required.")]
        public DateTime? OrderDeadline { get; set; }
        public Guid CategoryId { get; set; }
        public List<FoodImageDto>? FoodImages { get; set; }
    }
}
