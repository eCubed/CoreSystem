using CoreLibrary.AuthServer;
using CoreLibrary.Cryptography;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CoreSystem.AuthServer.Providers
{
    public class CoreSystemTokenIssuerMiddleware : TokenIssuerMiddlewareBase<CoreSystemAuthServerResponse>
    {
        public CoreSystemTokenIssuerMiddleware(RequestDelegate next, ICrypter crypter, TokenIssuerOptions tokenIssuerOptions)
            : base(next, crypter, tokenIssuerOptions)
        {
        }

        protected override void SetOtherAuthServerResponseProperties(AuthServerRequest authServerRequest, CoreSystemAuthServerResponse authServerResponse)
        {
            List<string> roles = new List<string>();
            roles.Add("user");

            if (authServerRequest.Username == "admin")
                roles.Add("admin");

            authServerResponse.Roles = roles;
        }
    }
}
