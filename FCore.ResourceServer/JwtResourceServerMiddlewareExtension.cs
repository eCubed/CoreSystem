using Microsoft.AspNetCore.Builder;

namespace FCore.ResourceServer
{
    public static class JwtResourceServerMiddlewareExtension
    {
        public static void UseJwtResourceServerMiddleware(this IApplicationBuilder app, JwtResourceServerOptions resourceServerOptions)
        {
            app.UseMiddleware<JwtResourceServerMiddleware>(resourceServerOptions);
        }
    }
}
