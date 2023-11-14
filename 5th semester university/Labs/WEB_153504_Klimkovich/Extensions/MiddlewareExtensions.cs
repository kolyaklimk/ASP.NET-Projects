namespace WEB_153504_Klimkovich.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this
        IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
