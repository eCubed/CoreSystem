using Microsoft.AspNetCore.Builder;

namespace FCore.ResourceServer
{
    public static class ResourceServerMiddlewareExtensions
    {
        public static IApplicationBuilder UseResourceServerMiddleware(this IApplicationBuilder app, ResourceServerOptions resourceServerOptions)
        {            
            return app.UseMiddleware<ResourceServerMiddleware>(resourceServerOptions);
        }

        public static IApplicationBuilder UseJwtResourceServerMiddleware(this IApplicationBuilder app, ResourceServerOptions resourceServerOptions)
        {
            return app.UseMiddleware<JwtResourceServerMiddleware>(resourceServerOptions);
        }

        public static IApplicationBuilder UseApiKeyMiddleware<TApiClient>(this IApplicationBuilder app, ApiKeyMiddlewareOptions apiKeyMiddlewareOptions = null)
            where TApiClient : class, IApiClient
        {
            return app.UseMiddleware<ApiKeyMiddleware<TApiClient>>(apiKeyMiddlewareOptions);
        }
    }
}
