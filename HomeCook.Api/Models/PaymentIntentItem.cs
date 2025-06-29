namespace HomeCook.Api.Models
{
    public class PaymentIntentItemData
    {
        public long  Amount { get; set; }
        public int Quantity { get; set; }
        public Guid FoodId { get; set; }
    }
}
