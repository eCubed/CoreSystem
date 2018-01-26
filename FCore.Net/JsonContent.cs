using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Text;

namespace FCore.Net
{
    public class JsonContent<T> : StringContent
    {
        public JsonContent(T value) : base(JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            }), Encoding.UTF8, "application/json")
        {
        }

        public JsonContent(T value, string mediaType) : base(JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            }), Encoding.UTF8, mediaType)
        {
        }
    }
}
