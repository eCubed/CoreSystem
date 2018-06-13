using Newtonsoft.Json;

namespace FCore.AuthServer
{
    public class AuthServerResponse : IAuthServerResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
