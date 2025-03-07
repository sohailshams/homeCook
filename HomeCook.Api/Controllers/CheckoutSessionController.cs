using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutSessionController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreatePaymentSession([FromBody] PaymentSessionItem request)
        {
            //var domain = "http://localhost:5173";
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            try
            {
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "gbp",
                        UnitAmount = request.Price, // Amount in cents (e.g., 100 = $1.00)
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = request.Name // Pass product name
                    }
                    },
                    Quantity = request.Quantity,
                  },
                },
                    Mode = "payment",
                    //SuccessUrl = baseUrl + "?success=true",
                      SuccessUrl = $"{baseUrl}/success",
                    CancelUrl = baseUrl,
                };
                var service = new SessionService();
                Session session = service.Create(options);

                Response.Headers.Append("Location", session.Url);
                return new StatusCodeResult(303);
            }

            catch (Exception exception)
            {
                throw new DatabaseOperationException("Failed to process payment.", exception);
            }
        }
    }
}
