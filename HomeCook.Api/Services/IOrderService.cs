using Stripe;

namespace HomeCook.Api.Services
{
    public interface IOrderService
    {
        Task ProcessAuthorizedPaymentAsync(PaymentIntent paymentIntent);
        Task ProcessCanceledPaymentAsync(PaymentIntent paymentIntent);
    }
}
