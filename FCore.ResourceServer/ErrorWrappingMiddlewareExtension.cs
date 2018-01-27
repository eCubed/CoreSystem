using Microsoft.AspNetCore.Builder;

namespace FCore.ResourceServer
{
    public static class ErrorWrappingMiddlewareExtension
    {
        public static void UseErrorWrappingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
