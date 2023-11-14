using Serilog;

namespace WEB_153504_Klimkovich
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            {
                Log.Information($"---> request {context.Request.Path} returns {context.Response.StatusCode}");
            }
        }
    }
}
