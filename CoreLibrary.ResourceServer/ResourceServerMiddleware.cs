using CoreLibrary.Cryptography;
using CoreLibrary.NetSecurity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                    BadRequestApiResponse response = new BadRequestApiResponse(ResourceServerMessages.InvalidAuthorizationHeader);
                    await context.Response.WriteAsync(response.JsonSerialize());
                    return;
                }

                if (values.First() != "Bearer")
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    BadRequestApiResponse response = new BadRequestApiResponse(ResourceServerMessages.AuthorizationBearerRequired);
                    await context.Response.WriteAsync(response.JsonSerialize());
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
                        UnauthorizedApiResponse response = new UnauthorizedApiResponse(ResourceServerMessages.IssuersDoNotMatch);
                        await context.Response.WriteAsync(response.JsonSerialize());
                        return;
                    }

                    DateTime tokenExpiration = webToken.CreatedDate.AddDays(_resourceServerOptions?.TokenDurationInDays ?? 14);

                    if (DateTime.Now > tokenExpiration)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json;charset=utf-8";
                        UnauthorizedApiResponse response = new UnauthorizedApiResponse(ResourceServerMessages.TokenExpired);
                        await context.Response.WriteAsync(response.JsonSerialize());
                        return;
                    }

                    // Now, we have to write the claims to the ClaimsPrincipal!
                    context.User = webToken.ConvertToClaimsPrincipal();
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    UnauthorizedApiResponse response = new UnauthorizedApiResponse(ResourceServerMessages.InvalidToken,
                        new List<string> { e.Message });
                    await context.Response.WriteAsync(response.JsonSerialize());
                    return;
                }
            }

            try
            {
                await _next.Invoke(context);
            }
            catch(Exception e)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json;charset=utf-8";
                UnauthorizedApiResponse response = new UnauthorizedApiResponse(ResourceServerMessages.ServerError,
                    new List<string> { e.Message });
                await context.Response.WriteAsync(response.JsonSerialize());
                return;
            }
        }
    }
}
