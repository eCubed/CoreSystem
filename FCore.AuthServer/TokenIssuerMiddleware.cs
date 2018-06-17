using FCore.Cryptography;
using FCore.Net.Security;
using Microsoft.AspNetCore.Http;

namespace FCore.AuthServer
{
    public class TokenIssuerMiddleware : TokenIssuerMiddlewareBase<AuthServerResponse, WebToken>
    {
        private ICrypter Crypter { get; set; }

        public TokenIssuerMiddleware(RequestDelegate next, ICrypter crypter, TokenIssuerOptions tokenIssuerOptions)
            : base(next, tokenIssuerOptions)
        {
            Crypter = crypter;
        }

        protected override IWebToken CreateStarterTokenInstance()
        {
            return new WebToken(Crypter);
        }
    }
}
