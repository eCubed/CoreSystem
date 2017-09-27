using CoreLibrary.AuthServer;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CoreSystem.AuthServer.Providers
{
    public class AdditionalClaimsProvider : IAdditionalClaimsProvider
    {
        public List<KeyValuePair<string, string>> GetAdditionalClientClaims(string clientId)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("companyId","100")
            };
        }

        public List<KeyValuePair<string, string>> GetAdditionalUserClaims(string username)
        {
            List<KeyValuePair<string, string>> userClaims = new List<KeyValuePair<string, string>>();

            userClaims.Add(new KeyValuePair<string, string>(ClaimTypes.Name, username));
            userClaims.Add(new KeyValuePair<string, string>(ClaimTypes.Role, "user"));

            if (username == "admin")
                userClaims.Add(new KeyValuePair<string, string>(ClaimTypes.Role, "admin"));

            return userClaims;
        }
    }
}
