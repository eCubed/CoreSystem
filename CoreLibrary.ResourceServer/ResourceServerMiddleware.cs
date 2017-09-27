using CoreLibrary.Cryptography;
using CoreLibrary.NetSecurity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLibrary.ResourceServer
{
    public class ResourceServerMiddleware
    {
        private RequestDelegate _next;
        private ICrypter _crypter;
        private ResourceServerOptions _resourceServerOptions;

        public ResourceServerMiddleware(RequestDelegate next, ICrypter crypter, ResourceServerOptions resourceServerOptions)
        {
            _next = next;
            _crypter = crypter;
            _resourceServerOptions = resourceServerOptions;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string[] values = context.Request.Headers["Authorization"].ToString().Split(' ');

                if (values.Length != 2)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    await context.Response.WriteAsync($"[\"{ResourceServerMessages.InvalidAuthorizationHeader}\"]");
                    return;
                }

                if (values.First() != "Bearer")
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    await context.Response.WriteAsync($"[\"{ResourceServerMessages.AuthorizationBearerRequired}\"]");
                    return;
                }

                try
                {
                    string webTokenJsonString = _crypter.Decrypt(values[1], _resourceServerOptions.CryptionKey);
                    WebToken webToken = JsonConvert.DeserializeObject<WebToken>(webTokenJsonString);

                    if (webToken.Issuer != _resourceServerOptions.Issuer)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json;charset=utf-8";
                        await context.Response.WriteAsync($"[\"{ResourceServerMessages.IssuersDoNotMatch}\"]");
                        return;
                    }

                    DateTime tokenExpiration = webToken.CreatedDate.AddDays(_resourceServerOptions?.TokenDurationInDays ?? 14);

                    if (DateTime.Now > tokenExpiration)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json;charset=utf-8";
                        await context.Response.WriteAsync($"[\"{ResourceServerMessages.TokenExpired}\"]");
                        return;
                    }

                    // Now, we have to write the claims to the ClaimsPrincipal!
                    context.User = webToken.ConvertToClaimsPrincipal();
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    await context.Response.WriteAsync($"[\"{e.Message}\"]");
                }
            }

            await _next.Invoke(context);
        }
    }
}
