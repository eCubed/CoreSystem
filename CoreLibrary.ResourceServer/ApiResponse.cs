using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/// <summary>
/// Courtesy: https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api
/// </summary>
namespace CoreLibrary.ResourceServer
{
    /// <summary>
    /// This is the base class of an HTTP response. Its children would include the common responses based on
    /// HttpStatusCode. This is typically used for error responses, but would also be used for 2xx responses.
    /// Doing this will create consistency for clients!
    /// </summary>
    public class ApiResponse
    {
        public int HttpStatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AppSpecificCode { get; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(int statusCode, string appSpecificCode = null, string message = null)
        {
            HttpStatusCode = statusCode;
            AppSpecificCode = appSpecificCode;
            Message = message;
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
