using CoreLibrary.AuthServer;
using CoreSystem.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CoreSystem.AuthServer.Providers
{
    public class PasswordCredentialsProvider : IPasswordCredentialsProvider
    {
        private UserManager<CoreSystemUser> UserManager;

        public PasswordCredentialsProvider(UserManager<CoreSystemUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task<bool> AreCredentialsValidAsync(string uniqueIdentifier, string passCode)
        {
            CoreSystemUser user = await UserManager.FindByNameAsync(uniqueIdentifier);
            if (user == null)
                return false;

            return await UserManager.CheckPasswordAsync(user, passCode);
        }
    }
}
