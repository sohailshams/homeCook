using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public required Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public required Guid FoodId { get; set; }
        public Food Food { get; set; } = null!;
        public int Quantity { get; set; }
        [Precision(10, 2)]
        public decimal Price { get; set; }
    }
}
