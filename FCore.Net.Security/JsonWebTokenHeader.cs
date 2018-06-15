using Newtonsoft.Json;

namespace FCore.Net.Security
{
    public class JsonWebTokenHeader
    {
        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("typ")]
        public string Type { get; set; }

        public JsonWebTokenHeader()
        {
        }

        public string GetJsonSerialization()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
