using Microsoft.AspNetCore.Http;

namespace CoreLibrary.AuthServer
{
    public class TokenIssuerMiddleware : TokenIssuerMiddlewareBase<AuthServerResponse>
    {
        public TokenIssuerMiddleware(RequestDelegate next, TokenIssuerOptions tokenIssuerOptions)
            : base(next, tokenIssuerOptions)
        {
        }
    }
}
