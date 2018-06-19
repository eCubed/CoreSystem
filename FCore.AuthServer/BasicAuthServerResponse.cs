using Newtonsoft.Json;

namespace FCore.AuthServer
{
    public class BasicAuthServerResponse : IAuthServerResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
