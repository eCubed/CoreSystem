using CoreLibrary.AuthServer;
using System.Threading.Tasks;

namespace CoreSystem.AuthServer.Providers
{
    public class SimpleCredentialsProvider : ICredentialsProvider
    {
        public async Task<bool> AreClientCredentialsValidAsync(string clientId, string secret)
        {
            return await Task.FromResult((clientId == "client"));
        }

        public async Task<bool> AreUserCredentialsValidAsync(string username, string password)
        {
            return await Task.FromResult((username == "admin") || (username.StartsWith("user")));
        }
    }
}
