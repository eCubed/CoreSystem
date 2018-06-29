using FCore.ResourceServer;

namespace FCore.ResourceServerTest.Models
{
    public class ApiClient : IApiClient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public string GetIdAsString()
        {
            return Id.ToString();
        }
    }
}
