using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Courtesy: https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api
/// </summary>
namespace CoreLibrary.ResourceServer
{  
    public class ErrorWrappingMiddleware
    {
        private RequestDelegate _next;

        public ErrorWrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError(EventIds.GlobalException, ex, ex.Message);

                context.Response.StatusCode = 500;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var badResponse = new BadRequestApiResponse("invalid-route", new List<string> { "invalid-route" });                
                await context.Response.WriteAsync(badResponse.JsonSerialize());
            }
        }
    }
}
