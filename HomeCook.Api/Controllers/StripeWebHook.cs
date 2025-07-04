﻿using HomeCook.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace HomeCook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebHook : ControllerBase
    {
        private readonly IConfiguration _conf;
        private readonly IUpdateQuantityService _updateQuantity;
        private readonly IEmailService _mailService;

        public StripeWebHook(IConfiguration conf, IUpdateQuantityService updateQuantity, IEmailService mailService)
        {
            _conf = conf;
            _updateQuantity = updateQuantity;
            _mailService = mailService;
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
                    var foodId = paymentIntent.Metadata["foodId"];
                    if (Guid.TryParse(foodId, out Guid parsedFoodId) && int.TryParse(purchasedItemQuantity, out int parsedQuantity))
                    {
                        var result = await _updateQuantity.UpdateQuantityAsync(parsedFoodId, parsedQuantity);
                        if (!result)
                        {
                            await _mailService.SendEmailAsync(Constants.Constants.EmailFrom, Constants.Constants.EmailTo, Constants.Constants.EmailSubject, Constants.Constants.PlainTextContent, Constants.Constants.HtmlContentBody);
                        }

                    }
                    else
                    {
                        await _mailService.SendEmailAsync(Constants.Constants.EmailFrom, Constants.Constants.EmailTo, Constants.Constants.EmailSubject, Constants.Constants.PlainTextContent, Constants.Constants.HtmlContentBody);
                    }
                }

            }
                return Ok(); 
        }
    }
}
