using Newtonsoft.Json;

namespace CoreLibrary.AuthServer
{
    public class AuthServerResponse : IAuthServerResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
