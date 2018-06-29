using FCore.Cryptography;
using FCore.ResourceServer;

namespace FCore.ResourceServerTest.Providers
{
    public class ApiClientHasher : IApiClientHasher
    {
        public bool CheckHash(string apiKey, string clientSecret, string xInputValue, string clientHash)
        {
            string hash = Hasher.GetHash($"{apiKey}.{clientSecret}.{xInputValue}", Hasher.HashType.MD5);

            return hash == clientHash;
        }
    }
}
