using System.Net;
using System.Text.Json;

namespace Project.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// 500 with <c>Content-Type: application/json</c> and the body <c>{"error":"..."}</c> (real JSON).
        /// Production: the message is always "Internal Server Error" — no exception text, no stack trace.
        /// Development: message + "\n" + stack trace.
        /// </summary>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var message = "Internal Server Error";
            if (context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
            {
                message = exception.Message + "\n" + exception.StackTrace;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
        }
    }
}
