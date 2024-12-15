namespace HomeCook.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public required string BuyerId { get; set; }
        public required Buyer Buyer { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
