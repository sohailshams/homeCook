using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePaymentIntent : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> PaymentIntent([FromBody] PaymentIntentItem paymentAmount)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = paymentAmount.Amount * 100,
                    Currency = "gbp",
                    AutomaticPaymentMethods = new()
                    {
                        Enabled = true,
                    }
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);
                return Ok(new { clientSecret = paymentIntent.ClientSecret });
            }

            catch (Exception exception)
            {
                throw new DatabaseOperationException("Failed to process payment.", exception);
            }
        }
    }
}
