
namespace SimpleLinkShrink.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (!context.Response.HasStarted && context.Response.StatusCode == 404)
            {
                context.Request.Path = "/404";
                await _next(context);
            }
        }
    }

    public static class NotFoundMiddlewareExtensions
    {
        public static IApplicationBuilder UseNotFound(this IApplicationBuilder app)
        {
            return app.UseMiddleware<NotFoundMiddleware>();
        }
    }
}
