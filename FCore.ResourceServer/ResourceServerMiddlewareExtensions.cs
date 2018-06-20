using Microsoft.AspNetCore.Builder;

namespace FCore.ResourceServer
{
    public static class ResourceServerMiddlewareExtensions
    {
        public static void UseResourceServerMiddleware(this IApplicationBuilder app, ResourceServerOptions resourceServerOptions)
        {            
            app.UseMiddleware<ResourceServerMiddleware>(resourceServerOptions);
        }

        public static void UseJwtResourceServerMiddleware(this IApplicationBuilder app, ResourceServerOptions resourceServerOptions)
        {
            app.UseMiddleware<JwtResourceServerMiddleware>(resourceServerOptions);
        }
    }
}
