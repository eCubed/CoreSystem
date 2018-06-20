using FCore.Net.Security;
using Microsoft.AspNetCore.Http;

namespace FCore.ResourceServer
{
    public class JwtResourceServerMiddleware : ResourceServerMiddlewareBase<JsonWebToken>
    {
        public JwtResourceServerMiddleware(RequestDelegate next, ResourceServerOptions resourceServerOptions)
            : base(next, resourceServerOptions)
        {
        }

        protected override JsonWebToken CreateStarterTokenInstance()
        {
            return new JsonWebToken();
        }
    }
}
