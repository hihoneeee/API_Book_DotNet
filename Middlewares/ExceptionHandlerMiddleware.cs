using System.Net;
using TestWebAPI.Helpers;

namespace TestWebAPI.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
       private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpStatusException ex)
            {
                _logger.LogError(ex, "A HTTP status exception occurred.");
                await HandleHttpStatusExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleHttpStatusExceptionAsync(HttpContext context, HttpStatusException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception._statusCode;

            var response = new
            {
                success = false,
                statusCode = exception._statusCode,
                message = "Internal Server Error"
            };

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                Message = "An error occurred while processing your request.",
                Detailed = exception.Message
            };

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }
}
