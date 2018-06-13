using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace FCore.ResourceServer
{
    public class ApiResponseBody<T>
    {
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponseBody()
        {
        }

        public ApiResponseBody(T data)
        {
            Data = data;
        }

        public ApiResponseBody(List<string> errors)
        {
            Errors = errors;
        }

        public ApiResponseBody(string error)
        {
            Errors = new List<string> { error };
        }

        public string JsonSerialize()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
