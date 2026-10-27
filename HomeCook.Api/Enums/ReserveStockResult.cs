namespace HomeCook.Api.Enums
{
    public enum ReserveStockResult
    {
        Created,        // stock claimed and order saved
        OutOfStock,     // not enough stock left, nothing changed
        OrderingClosed, // the food's order deadline has passed, nothing changed
        Duplicate       // an order for this payment already exists, nothing changed
    }
}
