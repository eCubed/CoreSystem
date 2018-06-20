using Microsoft.AspNetCore.Builder;

namespace FCore.AuthServer
{
    public static class TokenIssuerMiddlewareExtension
    {
        public static void UseTokenIssuerMiddleware(this IApplicationBuilder app, TokenIssuerOptions tokenIssuerOptions)
        {
            app.UseMiddleware<TokenIssuerMiddleware>(tokenIssuerOptions);
        }
    }
}
