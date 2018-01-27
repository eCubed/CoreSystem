using Microsoft.AspNetCore.Builder;

namespace FCore.ResourceServer
{
    public static class ResourceServerMiddlewareExtension
    {
        public static void UseResourceServerMiddleware(this IApplicationBuilder app, ResourceServerOptions resourceServerOptions)
        {            
            app.UseMiddleware<ResourceServerMiddleware>(resourceServerOptions);
        }
    }
}
