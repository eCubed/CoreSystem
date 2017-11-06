using CoreLibrary.AuthServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using CoreSystem.EntityFramework;
using System.Security.Claims;

namespace CoreSystem.AuthServer.Providers
{
    public class PasswordClaimsProvider : IPasswordClaimsProvider
    {
        private UserManager<CoreSystemUser> UserManager { get; set; }

        public PasswordClaimsProvider(UserManager<CoreSystemUser> userManager)
        {
            UserManager = userManager;
        }

        public List<KeyValuePair<string, string>> GetAdditionalClaims(string uniqueIdentifier)
        {
            CoreSystemUser user = UserManager.FindByNameAsync(uniqueIdentifier).Result;
            if (user != null)
            {
                IList<string> roles = UserManager.GetRolesAsync(user).Result;

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

        public List<KeyValuePair<string, string>> GetBasicClaims(string uniqueIdentifier, HttpRequest request)
        {
            //token.AddClaim(ClaimTypes.Name, ExtractUniqueIdentifier(request));
            List<KeyValuePair<string, string>> claims = new List<KeyValuePair<string, string>>();

            claims.Add(new KeyValuePair<string, string>(ClaimTypes.Name, uniqueIdentifier));
            if (!string.IsNullOrEmpty(request.Form["client_id"].ToString()))
                claims.Add(new KeyValuePair<string, string>("ClientId", request.Form["client_id"].ToString()));

            return claims;
        }
    }
}
