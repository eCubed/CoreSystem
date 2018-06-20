using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

/// <summary>
/// Courtesy: https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api
/// </summary>
namespace FCore.WebApiServerBase
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
                context.Response.StatusCode = 500;
                await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, 500, WebApiServerMessages.ServerError, ex);
            }

            if (!context.Response.HasStarted)
            {  
                await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, 400, WebApiServerMessages.InvalidRoute);
            }
        }
    }
}
