using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebHook : ControllerBase
    {
        private readonly IConfiguration _conf;

        public StripeWebHook(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpPost]
        public async Task<IActionResult> HandlePostPaymentSuccessAction()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeWbHookSecret = _conf["Stripe:WebHookSecret"];
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                stripeWbHookSecret
            );

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null) 
                { 
                    var purchasedItemQuantity = paymentIntent.Metadata["quantity"]; 
                    // TODO: Parse metadata and update your item quantity here
                }

            }

            return Ok();
        }
    }
}
