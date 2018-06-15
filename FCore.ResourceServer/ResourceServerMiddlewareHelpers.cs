using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FCore.ResourceServer
{
    public static class ResourceServerMiddlewareHelpers
    {
        public static async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string errorMessage, Exception e = null)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json;charset=utf-8";
            ApiResponseBody<Object> response =
                (e != null) ? new ApiResponseBody<Object>(new List<string> { e.Message, errorMessage }) :
                new ApiResponseBody<Object>(errorMessage);
            await context.Response.WriteAsync(response.JsonSerialize());
        }
    }
}
