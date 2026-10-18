using HomeCook.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace HomeCook.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
        public required Guid BuyerId { get; set; }
        public User Buyer { get; set; } = null!;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        // Unique - a Stripe webhook delivered twice finds this order and stops
        public required string StripePaymentIntentId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
