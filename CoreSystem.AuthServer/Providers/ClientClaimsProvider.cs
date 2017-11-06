using CoreLibrary.AuthServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CoreSystem.AuthServer.Providers
{
    public class ClientClaimsProvider : IClientClaimsProvider
    {
        public ClientClaimsProvider()
        {
        }

        public List<KeyValuePair<string, string>> GetAdditionalClaims(string uniqueIdentifier)
        {
            return new List<KeyValuePair<string, string>>();
        }

        public List<KeyValuePair<string, string>> GetBasicClaims(string uniqueIdentifier, HttpRequest request)
        {
            List<KeyValuePair<string, string>> claims = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrEmpty(request.Form["client_id"]))
                //token.AddClaim("ClientId", request.Form["client_id"].ToString());
                claims.Add(new KeyValuePair<string, string>("ClientId", request.Form["client_id"].ToString()));

            return claims;
        }
    }
}
