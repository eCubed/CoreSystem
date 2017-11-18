using CoreLibrary.Net;
using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = WebApiClient.GetAsync<object>("http://localhost:49550/api/values").Result;

            var dummy = 3;
        }
    }
}
