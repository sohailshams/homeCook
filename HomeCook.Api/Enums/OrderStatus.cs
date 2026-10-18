namespace HomeCook.Api.Enums
{
    public enum OrderStatus
    {
        PendingCapture, // stock claimed, card on hold, not charged yet
        Paid,           // payment captured
        Cancelled       // hold released, stock restored
    }
}
