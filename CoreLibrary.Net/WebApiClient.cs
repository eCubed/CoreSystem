using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.Net
{
    public static class WebApiClient
    {
        private static void AddHeadersToHttpClient(HttpClient httpClient, List<KeyValuePair<string, string>> headers)
        {
            headers.ForEach(headerKvp =>
            {
                httpClient.DefaultRequestHeaders.Add(headerKvp.Key, headerKvp.Value);
            });
        }

        private static List<KeyValuePair<string, string>> CreateHeadersWithAccessToken(string accessToken, 
            List<KeyValuePair<string, string>> otherHeaders = null)
        {
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
            headers.Add(new KeyValuePair<string, string>("Authorization", $"Bearer {accessToken}"));

            if (otherHeaders != null)
            {
                headers.AddRange(otherHeaders);
            }

            return headers;
        }

        public static async Task<T> GetAsync<T>(string url, List<KeyValuePair<string, string>> headers = null)
            where T : class
        {
            using (HttpClient httpClient = new HttpClient())
            {
                if (headers != null)
                    AddHeadersToHttpClient(httpClient, headers);

                var jsonString = await httpClient.GetStringAsync(url);

                return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
        }

        public static async Task<T> GetAsync<T>(string url, string accessToken, List<KeyValuePair<string, string>> otherHeaders = null)
            where T : class
        {
            return await GetAsync<T>(url, CreateHeadersWithAccessToken(accessToken, otherHeaders));
        }

        private static async Task<object> SaveAsync<T>(string postOrPut, string url, T body, List<KeyValuePair<string, string>> headers = null)
           where T : class
        {
            using (HttpClient httpClient = new HttpClient())
            {
                if (headers != null)
                    AddHeadersToHttpClient(httpClient, headers);

                var json = JsonConvert.SerializeObject(body, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });

                HttpResponseMessage responseMesssage = (postOrPut == "post") ? await httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json")) :
                    await httpClient.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

                string jsonResponse = await responseMesssage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject(jsonResponse);
            }
        }

        private static async Task<object> SaveAsync<T>(string postOrPut, string url, string accessToken, T body, List<KeyValuePair<string, string>> otherHeaders = null)
            where T : class
        {
            return await SaveAsync<T>(postOrPut, url, body, CreateHeadersWithAccessToken(accessToken, otherHeaders));
        }


        public static async Task<object> PostAsync<T>(string url, T body, List<KeyValuePair<string, string>> headers)
            where T : class
        {
            return await SaveAsync("post", url, body, headers);
        }

        public static async Task<object> PostAsync<T>(string url, string accessToken, T body, List<KeyValuePair<string, string>> otherHeaders = null)
            where T : class
        {
            return await PostAsync<T>(url, body, CreateHeadersWithAccessToken(accessToken, otherHeaders));
        }

        public static async Task<object> PutAsync<T>(string url, T body, List<KeyValuePair<string, string>> headers = null)
            where T : class
        {
            return await SaveAsync("put", url, body, headers);
        }

        public static async Task<object> PutAsync<T>(string url, string accessToken, T body, List<KeyValuePair<string, string>> otherHeaders = null)
            where T : class
        {
            return await PutAsync<T>(url, body, CreateHeadersWithAccessToken(accessToken, otherHeaders));
        }
    }
}
