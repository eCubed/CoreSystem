using CoreLibrary.AuthServer;
using System.Threading.Tasks;

namespace CoreSystem.AuthServer.Providers
{
    public class ClientCredentialsProvider : IClientCredentialsProvider
    {
        public ClientCredentialsProvider()
        {
        }

        public async Task<bool> AreCredentialsValidAsync(string uniqueIdentifier, string passCode)
        {
            return await Task.FromResult<bool>(true);
        }
    }
}
