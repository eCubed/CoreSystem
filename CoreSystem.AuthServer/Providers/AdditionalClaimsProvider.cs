using CoreLibrary.AuthServer;
using CoreSystem.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace CoreSystem.AuthServer.Providers
{
    public class AdditionalClaimsProvider : IAdditionalClaimsProvider
    {
        private UserManager<CoreSystemUser> userManager { get; set; }

        public AdditionalClaimsProvider(UserManager<CoreSystemUser> userManager)
        {
            this.userManager = userManager;
        }

        public List<KeyValuePair<string, string>> GetAdditionalClientClaims(string clientId)
        {
            return null;
        }

        public List<KeyValuePair<string, string>> GetAdditionalUserClaims(string username)
        {
            CoreSystemUser user = userManager.FindByNameAsync(username).Result;
            if (user != null)
            {
                IList<string> roles = userManager.GetRolesAsync(user).Result;

                List<KeyValuePair<string, string>> claims = new List<KeyValuePair<string, string>>();

                // Add roles first
                foreach (string role in roles)
                {
                    claims.Add(new KeyValuePair<string, string>(ClaimTypes.Role, role));
                }

                return claims;
            }

            return null;
        }
    }
}
