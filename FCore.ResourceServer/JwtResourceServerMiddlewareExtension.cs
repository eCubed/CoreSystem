using Microsoft.AspNetCore.Builder;

namespace FCore.ResourceServer
{
    public static class JwtResourceServerMiddlewareExtension
    {
        public static void UseJwtResourceServerMiddleware(this IApplicationBuilder app, ResourceServerOptions resourceServerOptions)
        {
            app.UseMiddleware<JwtResourceServerMiddleware>(resourceServerOptions);
        }
    }
}
