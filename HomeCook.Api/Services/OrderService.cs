using HomeCook.Api.EntityFramework.Repositories;
using HomeCook.Api.Enums;
using HomeCook.Api.Models;
using Stripe;

namespace HomeCook.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<OrderService> _logger;
        private readonly PaymentIntentService _paymentIntentService = new();

        public OrderService(IOrderRepository orderRepository, IEmailService emailService, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _emailService = emailService;
            _logger = logger;
        }

        // payment_intent.amount_capturable_updated: card is on hold, claim stock then charge
        public async Task ProcessAuthorizedPaymentAsync(PaymentIntent paymentIntent)
        {
            var existingOrder = await _orderRepository.GetByPaymentIntentIdAsync(paymentIntent.Id);
            if (existingOrder != null)
            {
                // A previous delivery saved the order but failed before capturing; finish the capture
                if (existingOrder.Status == OrderStatus.PendingCapture)
                {
                    await CaptureAsync(existingOrder);
                    return;
                }

                _logger.LogInformation($"Payment {paymentIntent.Id} already handled (order {existingOrder.Id} is {existingOrder.Status}); ignoring duplicate webhook.");
                return;
            }

            var metadata = ReadMetadata(paymentIntent);
            if (metadata == null)
            {
                _logger.LogError($"Payment {paymentIntent.Id} has missing or invalid metadata; cancelling it.");
                await CancelPaymentIntentAsync(paymentIntent.Id);
                await _emailService.SendEmailAsync(Constants.Constants.EmailFrom, Constants.Constants.EmailTo, Constants.Constants.EmailSubject, Constants.Constants.PlainTextContent, Constants.Constants.HtmlContentBody);
                return;
            }

            var (foodId, quantity, buyerId) = metadata.Value;

            var orderId = Guid.NewGuid();
            var totalPrice = paymentIntent.Amount / 100m;
            var order = new Order
            {
                Id = orderId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
                BuyerId = buyerId,
                StripePaymentIntentId = paymentIntent.Id,
                Status = OrderStatus.PendingCapture,
                Items =
                [
                    new OrderItem
                    {
                        OrderId = orderId,
                        FoodId = foodId,
                        Quantity = quantity,
                        Price = Math.Round(totalPrice / quantity, 2, MidpointRounding.AwayFromZero)
                    }
                ]
            };

            var result = await _orderRepository.ReserveStockAndCreateOrderAsync(order, foodId, quantity);

            switch (result)
            {
                case ReserveStockResult.OutOfStock:
                    // Sold out between checkout and payment: release the hold, buyer is never charged
                    _logger.LogWarning($"Food {foodId} sold out before payment {paymentIntent.Id} could claim {quantity} item(s) - cancelling payment.");
                    await CancelPaymentIntentAsync(paymentIntent.Id);
                    return;

                case ReserveStockResult.Duplicate:
                    _logger.LogInformation($"Payment {paymentIntent.Id} is being handled by another webhook delivery - ignoring.");
                    return;
            }

            await CaptureAsync(order);
        }

        // payment_intent.canceled: hold expired or was cancelled before capture, give the stock back
        public async Task ProcessCanceledPaymentAsync(PaymentIntent paymentIntent)
        {
            var order = await _orderRepository.GetByPaymentIntentIdAsync(paymentIntent.Id);
            if (order == null || order.Status != OrderStatus.PendingCapture)
                return;

            await _orderRepository.CancelOrderAndRestoreStockAsync(order);
            _logger.LogWarning($"Payment {paymentIntent.Id} was cancelled before capture; order {order.Id} cancelled and stock restored.");
        }

        private async Task CaptureAsync(Order order)
        {
            // Idempotency key: if this capture is retried, Stripe returns the first result instead of charging again.
            // If Stripe fails, the exception occurs, the webhook returns an error and Stripe retries.
            await _paymentIntentService.CaptureAsync(
                order.StripePaymentIntentId,
                null,
                new RequestOptions { IdempotencyKey = $"capture-{order.StripePaymentIntentId}" });

            await _orderRepository.UpdateStatusAsync(order, OrderStatus.Paid);
            _logger.LogInformation($"Captured payment {order.StripePaymentIntentId}; order {order.Id} is paid.");
        }

        private async Task CancelPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                await _paymentIntentService.CancelAsync(paymentIntentId);
            }
            catch (StripeException exception) when (exception.StripeError?.Code == "payment_intent_unexpected_state")
            {
                // Already cancelled or captured; nothing to do
                _logger.LogInformation($"Payment {paymentIntentId} could not be cancelled: {exception.Message}");
            }
        }

        private static (Guid FoodId, int Quantity, Guid BuyerId)? ReadMetadata(PaymentIntent paymentIntent)
        {
            if (paymentIntent.Metadata.TryGetValue("foodId", out var foodIdValue) && Guid.TryParse(foodIdValue, out var foodId)
                && paymentIntent.Metadata.TryGetValue("quantity", out var quantityValue) && int.TryParse(quantityValue, out var quantity) && quantity > 0
                && paymentIntent.Metadata.TryGetValue("buyerId", out var buyerIdValue) && Guid.TryParse(buyerIdValue, out var buyerId))
            {
                return (foodId, quantity, buyerId);
            }

            return null;
        }
    }
}
