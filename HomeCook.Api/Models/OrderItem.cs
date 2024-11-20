namespace HomeCook.Api.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public required Order Order { get; set; }
        public Guid FoodId { get; set; }
        public required Food Food { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
