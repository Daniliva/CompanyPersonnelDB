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
            throw new NotImplementedException("TODO");
        }
    }
}
