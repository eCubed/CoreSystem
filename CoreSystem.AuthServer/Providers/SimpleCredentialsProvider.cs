using CoreLibrary.AuthServer;
using CoreSystem.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace CoreSystem.AuthServer.Providers
{
    public class SimpleCredentialsProvider : ICredentialsProvider
    {
        private UserManager<CoreSystemUser> _userManager;

        public SimpleCredentialsProvider(UserManager<CoreSystemUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> AreClientCredentialsValidAsync(string clientId, string secret)
        {
            throw new NotSupportedException();
        }

        public async Task<bool> AreUserCredentialsValidAsync(string username, string password)
        {
            CoreSystemUser user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
