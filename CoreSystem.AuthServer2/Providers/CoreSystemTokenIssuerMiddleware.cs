using CoreLibrary.AuthServer;
using CoreLibrary.Cryptography;
using CoreSystem.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace CoreSystem.AuthServer2.Providers
{
    public class CoreSystemTokenIssuerMiddleware : TokenIssuerMiddlewareBase<CoreSystemAuthServerResponse>
    {
        private UserManager<CoreSystemUser> um { get; set; }

        public CoreSystemTokenIssuerMiddleware(UserManager<CoreSystemUser> userManager, RequestDelegate next, ICrypter crypter, TokenIssuerOptions tokenIssuerOptions)
            : base(next, crypter, tokenIssuerOptions)
        {
            um = userManager;
        }

        protected override void SetOtherAuthServerResponseProperties(AuthServerRequest authRequest, CoreSystemAuthServerResponse authServerResponse)
        {
            CoreSystemUser user = um.FindByNameAsync(authRequest.Username).Result;
            authServerResponse.Roles = um.GetRolesAsync(user).Result.ToList();
        }
    }
}
