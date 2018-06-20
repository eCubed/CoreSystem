using FCore.Cryptography;
using FCore.Net.Security;
using Microsoft.AspNetCore.Http;

namespace FCore.ResourceServer
{
    public class ResourceServerMiddleware : ResourceServerMiddlewareBase<WebToken>
    {
        private ICrypter Crypter;

        public ResourceServerMiddleware(RequestDelegate next, ICrypter crypter, ResourceServerOptions resourceServerOptions)
            : base(next, resourceServerOptions)
        {
            Crypter = crypter;
        }

        protected override WebToken CreateStarterTokenInstance()
        {
           return new WebToken(Crypter);
        }
    }
}
