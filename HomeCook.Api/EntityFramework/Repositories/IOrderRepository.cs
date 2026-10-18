using HomeCook.Api.Enums;
using HomeCook.Api.Models;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByPaymentIntentIdAsync(string paymentIntentId);
        Task<ReserveStockResult> ReserveStockAndCreateOrderAsync(Order order, Guid foodId, int quantity);
        Task UpdateStatusAsync(Order order, OrderStatus status);
        Task CancelOrderAndRestoreStockAsync(Order order);
    }
}
