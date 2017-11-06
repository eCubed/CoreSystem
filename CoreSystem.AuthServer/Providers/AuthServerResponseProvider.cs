using CoreLibrary.AuthServer;
using CoreSystem.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreSystem.AuthServer.Providers
{
    public class AuthServerResponseProvider : IAuthServerResponseProvider<CoreSystemAuthServerResponse>
    {
        private UserManager<CoreSystemUser> UserManager { get; set; } 

        public AuthServerResponseProvider(UserManager<CoreSystemUser> userManager)
        {
        }

        public void FillValues(string uniqueIdentifier, CoreSystemAuthServerResponse authServerResponse)
        {
            CoreSystemUser user = UserManager.FindByNameAsync(uniqueIdentifier).Result;

            if (user != null)
            {
                IList<string> roles = UserManager.GetRolesAsync(user).Result;

                authServerResponse.Roles = roles.ToList();
            }

            throw new NotImplementedException();
        }
    }
}
