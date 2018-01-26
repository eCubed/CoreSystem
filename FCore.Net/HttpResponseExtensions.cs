using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FCore.Net
{
    public static class HttpResponseExtensions
    {
        public static async Task<T> ContentAsTypeAsync<T>(this HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();

            return string.IsNullOrEmpty(data) ? default(T) :
                JsonConvert.DeserializeObject<T>(data);
        }

        // What about content as bytes!?
    }
}
