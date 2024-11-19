namespace HomeCook.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid RecipeId { get; set; }
        public Buyer Buyer { get; set; }
        public Seller Seller { get; set; }
        public Recipe Recipe { get; set; }
    }
}
