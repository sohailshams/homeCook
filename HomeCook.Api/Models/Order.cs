﻿namespace HomeCook.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid BuyerId { get; set; }
        public Guid SellerId { get; set; }
        public Guid FoodId { get; set; }
        public required Buyer Buyer { get; set; }
        public required Seller Seller { get; set; }
        public required Food Food { get; set; }
    }
}
