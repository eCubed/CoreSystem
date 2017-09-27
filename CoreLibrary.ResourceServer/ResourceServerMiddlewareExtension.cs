using Microsoft.AspNetCore.Builder;

namespace CoreLibrary.ResourceServer
{
    public static class ResourceServerMiddlewareExtension
    {
        public static void UseResourceServerMiddleware(this IApplicationBuilder app, ResourceServerOptions resourceServerOptions)
        {            
            app.UseMiddleware<ResourceServerMiddleware>(resourceServerOptions);
        }
    }
}
