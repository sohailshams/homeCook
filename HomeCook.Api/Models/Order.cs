namespace HomeCook.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public required Guid BuyerId { get; set; }
        public User Buyer { get; set; } = null!;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
