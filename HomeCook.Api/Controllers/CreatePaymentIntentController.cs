using HomeCook.Api.Exceptions;
using HomeCook.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePaymentIntentController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> PaymentIntent([FromBody] PaymentIntentItemData itemData)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = itemData.Amount * 100,
                    Currency = "gbp",
                    AutomaticPaymentMethods = new()
                    {
                        Enabled = true,
                    },
                    Metadata = new Dictionary<string, string>
                        {
                            { "quantity", itemData.Quantity.ToString() },
                            { "foodId", itemData.FoodId.ToString() }
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
