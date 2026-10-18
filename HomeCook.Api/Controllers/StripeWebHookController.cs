using HomeCook.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebHookController : ControllerBase
    {
        private readonly IConfiguration _conf;
        private readonly IOrderService _orderService;
        private readonly ILogger<StripeWebHookController> _logger;

        public StripeWebHookController(IConfiguration conf, IOrderService orderService, ILogger<StripeWebHookController> logger)
        {
            _conf = conf;
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessStripeEvent()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeWebHookSecret = _conf["Stripe:WebHookSecret"];

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    stripeWebHookSecret
                );
            }
            catch (StripeException exception)
            {
                // Invalid signature or payload
                _logger.LogWarning($"Rejected Stripe webhook: {exception.Message}");
                return BadRequest();
            }

            if (stripeEvent.Data.Object is not PaymentIntent paymentIntent)
                return Ok();

            // If processing fails, the API returns an error and Stripe sends the event again later.
            // Receiving the same event twice is safe, because OrderService skips payments it has already processed.
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentAmountCapturableUpdated:
                    await _orderService.ProcessAuthorizedPaymentAsync(paymentIntent);
                    break;

                case EventTypes.PaymentIntentCanceled:
                    await _orderService.ProcessCanceledPaymentAsync(paymentIntent);
                    break;
            }

            return Ok();
        }
    }
}
