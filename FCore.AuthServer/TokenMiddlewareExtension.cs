﻿using FCore.Net.Security;
using Microsoft.AspNetCore.Builder;

namespace FCore.AuthServer
{
    public static class TokenIssuerMiddlewareExtension
    {
        public static void UseTokenIssuerMiddleware(this IApplicationBuilder app, TokenIssuerOptions tokenIssuerOptions)
        {
            app.UseMiddleware<TokenIssuerMiddleware>(tokenIssuerOptions);
        }

        public static void UseTokenIssuerMiddleware<TTokenIssuerMiddleware, TAuthServerResponse>(this IApplicationBuilder app, TokenIssuerOptions tokenIssuerOptions)
            where TAuthServerResponse : class, IAuthServerResponse, new()
            where TTokenIssuerMiddleware : TokenIssuerMiddlewareBase<TAuthServerResponse, WebToken>
        {
            app.UseMiddleware<TTokenIssuerMiddleware>(tokenIssuerOptions);
        }

        public static void UseNewTokenIssuerMiddleware(this IApplicationBuilder app, TokenIssuerOptions tokenIssuerOptions)
        {
            app.UseMiddleware<NewTokenIssuerMiddleware>(tokenIssuerOptions);
        }
    }
}
