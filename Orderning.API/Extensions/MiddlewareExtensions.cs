using Ordering.API.Infrastructure.Middlewares;

namespace Ordening.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseByPassAuthMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ByPassAuthMiddleware>();
        }
       
    }
}
