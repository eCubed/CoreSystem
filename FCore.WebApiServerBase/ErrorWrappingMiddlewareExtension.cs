using Microsoft.AspNetCore.Builder;

namespace FCore.WebApiServerBase
{
    public static class ErrorWrappingMiddlewareExtension
    {
        public static void UseErrorWrappingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
