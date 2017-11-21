using CoreLibrary.Net;
using Tests.Entities;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //var result = WebApiClient.GetAsync<object>("http://localhost:49550/api/values").Result;
            //var result = HttpRequestFactory.GetAsync("http://localhost:49550/api/values").Result;
            //var result = HttpRequestFactory.GetAsync<object>("http://localhost:49550/api/values").Result;

            //var result = HttpRequestFactory.GetTokenAsync<object>("http://localhost:49197/token", "admin", "Aaa000$", "password").Result;

            //var result = HttpRequestFactory.UploadAsync<object>("http://localhost:49943/api/upload", "C:/FlixMLFiles/2017/10/201710172208120017_paella001.jpg").Result;

            var dummy = 3;
            EntitiesTests.CreateTest();
        }
    }
}
