using FCore.Cryptography;
using FCore.Net.Security;
using FCore.WebApiServerBase;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.ResourceServer
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
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, ResourceServerMessages.InvalidAuthorizationHeader);
                    return;
                }

                if (values.First() != "Bearer")
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, ResourceServerMessages.AuthorizationBearerRequired);
                    return;
                }

                try
                {
                    string webTokenJsonString = _crypter.Decrypt(values[1], _resourceServerOptions.CryptionKey);
                    WebToken webToken = JsonConvert.DeserializeObject<WebToken>(webTokenJsonString);

                    if (webToken.Issuer != _resourceServerOptions.Issuer)
                    {
                        await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.IssuersDoNotMatch);
                        return;
                    }

                    DateTime tokenExpiration = webToken.CreatedDate.AddDays(_resourceServerOptions?.TokenDurationInDays ?? 14);

                    if (DateTime.Now > tokenExpiration)
                    {
                        await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.TokenExpired);
                        return;
                    }
                    
                    context.User = webToken.ConvertToClaimsPrincipal();
                }
                catch (Exception e)
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.InvalidToken, e);
                    return;
                }
            }

            try
            {
                await _next.Invoke(context);
            }
            catch(InvalidOperationException e)
            {
                await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.InsufficientCredentials, e);
                return;
            }
            catch(Exception e)
            {
                await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError, WebApiServerMessages.ServerError, e);
                return;
            }
        }
    }
}
