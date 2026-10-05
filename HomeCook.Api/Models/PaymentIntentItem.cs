namespace HomeCook.Api.Models
{
    public class PaymentIntentItemData
    {
        public int Quantity { get; set; }
        public Guid FoodId { get; set; }
    }
}
