using FCore.ResourceServer;
using FCore.ResourceServerTest.Models;

namespace FCore.ResourceServerTest.Providers
{
    public class ApiClientProvider : IApiClientProvider<ApiClient>
    {
        public bool ClientExists(string clientId)
        {
            return true;
        }

        public ApiClient GetClientByPublicKey(string publicKey)
        {
            return new ApiClient
            {
                Id = 7,
                PublicKey = "XAXA",
                PrivateKey = "sana'y wala nang wakas",
                Name = "Sharon Cuneta"
            };
        }

        public string GetClientSecret(string clientId)
        {
            return "sana'y wala nang wakas";
        }
    }
}
