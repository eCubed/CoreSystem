using CoreLibrary.AuthServer;
using CoreSystem.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CoreSystem.AuthServer.Providers
{
    public class CoreSystemTokenIssuerMiddleware : TokenIssuerMiddlewareBase<CoreSystemAuthServerResponse>
    {
        private UserManager<CoreSystemUser> um { get; set; }

        public CoreSystemTokenIssuerMiddleware(UserManager<CoreSystemUser> userManager, RequestDelegate next, TokenIssuerOptions tokenIssuerOptions)
            : base(next, tokenIssuerOptions)
        {
            um = userManager;
        }

        /*
        protected override void SetOtherAuthServerResponseProperties(AuthServerRequest authRequest, CoreSystemAuthServerResponse authServerResponse)
        {
            CoreSystemUser user = um.FindByNameAsync(authRequest.Username).Result;
            authServerResponse.Roles = um.GetRolesAsync(user).Result.ToList();
        }
        */
    }
}
