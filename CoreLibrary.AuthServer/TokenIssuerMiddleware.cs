using CoreLibrary.Cryptography;
using Microsoft.AspNetCore.Http;

namespace CoreLibrary.AuthServer
{
    public class TokenIssuerMiddleware : TokenIssuerMiddlewareBase<AuthServerResponse>
    {
        public TokenIssuerMiddleware(RequestDelegate next, ICrypter crypter, TokenIssuerOptions tokenIssuerOptions)
            : base(next, crypter, tokenIssuerOptions)
        {
        }
    }
}
