using Newtonsoft.Json;

namespace CoreLibrary.AuthServer
{
    public class AuthServerRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; } // password or client
    }
}
