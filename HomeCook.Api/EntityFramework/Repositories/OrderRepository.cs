using HomeCook.Api.Enums;
using HomeCook.Api.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HomeCook.Api.EntityFramework.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order?> GetByPaymentIntentIdAsync(string paymentIntentId)
        {
            return await _dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.StripePaymentIntentId == paymentIntentId);
        }

        public async Task<ReserveStockResult> ReserveStockAndCreateOrderAsync(Order order, Guid foodId, int quantity)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            // Single SQL UPDATE: checks and decrements stock atomically, so two buyers can't both take the last portion
            var rowsUpdated = await _dbContext.Foods
                .Where(f => f.Id == foodId && f.QuantityAvailable >= quantity)
                .ExecuteUpdateAsync(s => s.SetProperty(f => f.QuantityAvailable, f => f.QuantityAvailable - quantity));

            if (rowsUpdated == 0)
            {
                await transaction.RollbackAsync();
                return ReserveStockResult.OutOfStock;
            }

            try
            {
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                // Another delivery of the same webhook already created this order; undo our stock decrement
                await transaction.RollbackAsync();
                _dbContext.Entry(order).State = EntityState.Detached;
                return ReserveStockResult.Duplicate;
            }

            await transaction.CommitAsync();
            return ReserveStockResult.Created;
        }

        public async Task UpdateStatusAsync(Order order, OrderStatus status)
        {
            order.Status = status;
            await _dbContext.SaveChangesAsync();
        }

        public async Task CancelOrderAndRestoreStockAsync(Order order)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            foreach (var item in order.Items)
            {
                await _dbContext.Foods
                    .Where(f => f.Id == item.FoodId)
                    .ExecuteUpdateAsync(s => s.SetProperty(f => f.QuantityAvailable, f => f.QuantityAvailable + item.Quantity));
            }

            order.Status = OrderStatus.Cancelled;
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
    }
}
