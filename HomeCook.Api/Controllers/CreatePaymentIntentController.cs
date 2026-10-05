using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using HomeCook.Api.Models;
using HomeCook.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePaymentIntentController : ControllerBase
    {
        private readonly ILogger<CreatePaymentIntentController> _logger;
        private readonly IFoodService _foodService;

        public CreatePaymentIntentController(
            ILogger<CreatePaymentIntentController> logger,
            IFoodService foodService
            )
        {
            _logger = logger;
            _foodService = foodService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PaymentIntent([FromBody] PaymentIntentItemData itemData)
        {
            _logger.LogInformation($"PaymentIntent - Creating payment intent for FoodId {itemData.FoodId}, quantity {itemData.Quantity}");

            if (itemData.Quantity <= 0)
                throw new ValidationException("Quantity must be at least 1.");

            var food = await _foodService.GetFoodDetailAsync(itemData.FoodId);

            if (itemData.Quantity > food!.QuantityAvailable)
                throw new ValidationException($"Only {food.QuantityAvailable} item(s) available.");

            // Stripe expects the amount in the smallest currency unit (pence)
            var amountInPence = (long)Math.Round(food.Price * itemData.Quantity * 100m, MidpointRounding.AwayFromZero);

            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var options = new PaymentIntentCreateOptions
            {
                Amount = amountInPence,
                Currency = "gbp",
                AutomaticPaymentMethods = new() { Enabled = true },
                Metadata = new Dictionary<string, string>
                {
                    { "quantity", itemData.Quantity.ToString() },
                    { "foodId", itemData.FoodId.ToString() },
                    { "buyerId", buyerId ?? string.Empty }
                }
            };

            var paymentIntent = await new PaymentIntentService().CreateAsync(options);
            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
    }
}
