using CoreLibrary.AuthServer;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoreSystem.AuthServer2.Providers
{
    public class CoreSystemAuthServerResponse : AuthServerResponse
    {
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
    }
}
