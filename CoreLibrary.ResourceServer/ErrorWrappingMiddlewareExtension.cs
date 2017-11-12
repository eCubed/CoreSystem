using Microsoft.AspNetCore.Builder;

namespace CoreLibrary.ResourceServer
{
    public static class ErrorWrappingMiddlewareExtension
    {
        public static void UseErrorWrappingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
