using System.ComponentModel.DataAnnotations;
using Stripe;

namespace HomeCook.Api.Exceptions
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed to the next middleware
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Default response
            context.Response.ContentType = "application/json";

            int statusCode;
            string errorMessage;

            // Handle exceptions based on their type
            switch (exception)
            {
                case DatabaseOperationException ex:
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorMessage = ex.Message;
                    break;

                case ValidationException dbEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    errorMessage = dbEx.Message;
                    break;

                case UnauthorizedAccessException dbEx:
                    statusCode = StatusCodes.Status401Unauthorized;
                    errorMessage = dbEx.Message;
                    break;

                case NotFoundException dbEx:
                    statusCode = StatusCodes.Status404NotFound;
                    errorMessage = dbEx.Message;
                    break;

                case EntityAlreadyExistsException dbEx:
                    statusCode = StatusCodes.Status409Conflict;
                    errorMessage = dbEx.Message;
                    break;

                case HttpRequestException:
                    statusCode = StatusCodes.Status502BadGateway;
                    errorMessage = "An external service is unavailable. Please try again.";
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorMessage = "An unexpected error occurred.";
                    break;

                case StripeException stripeEx:
                    var isCardError = stripeEx.StripeError?.Type == "card_error";
                    statusCode = isCardError ? StatusCodes.Status402PaymentRequired : StatusCodes.Status502BadGateway;
                    errorMessage = isCardError
                        ? stripeEx.StripeError!.Message
                        : "Payment service is unavailable. Please try again.";
                    break;
            }

            if (statusCode >= 500)
            {
                // Server side failure: log the full exception, stack trace and inner exception
                _logger.LogError(exception, $"Unhandled exception for {context.Request.Method} {context.Request.Path}");
            }
            else
            {
                // Client error (400/401/404/409): expected, a short line is enough
                _logger.LogWarning($"{statusCode} for {context.Request.Method} {context.Request.Path}: {exception.Message}");
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = errorMessage ?? "An error occurred." // Fallback to avoid null
            };

            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }

}
