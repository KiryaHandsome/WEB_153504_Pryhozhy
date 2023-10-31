using Serilog;

namespace WEB_153504_Pryhozhy
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            var response = context.Response;
            var statusCode = response.StatusCode;

            if (statusCode < 200 || statusCode >= 300)
            {
                var request = context.Request;
                var message = $"{DateTimeOffset.Now} [{context.TraceIdentifier}] ---> request {request.Path}{request.QueryString} returns {statusCode}";

                Log.Logger.Information(message);
            }
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
