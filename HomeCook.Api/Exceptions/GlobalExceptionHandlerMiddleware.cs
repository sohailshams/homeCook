using System.ComponentModel.DataAnnotations;

namespace HomeCook.Api.Exceptions
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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
                case DatabaseOperationException dbEx:
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorMessage = dbEx.Message;
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

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorMessage = "An unexpected error occurred.";
                    break;
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
