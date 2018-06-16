using FCore.Net.Security;
using FCore.WebApiServerBase;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.ResourceServer
{
    public abstract class ResourceServerMiddlewareBase<TWebToken>
        where TWebToken : class, IWebToken, new()
    { 
        protected RequestDelegate Next;
        protected ResourceServerOptions ResourceServerOptions;

        public ResourceServerMiddlewareBase(RequestDelegate next, ResourceServerOptions resourceServerOptions)
        {
            Next = next;
            ResourceServerOptions = resourceServerOptions;
        }

        protected abstract TWebToken CreateStarterTokenInstance();

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
                    IWebToken webToken = CreateStarterTokenInstance().Parse(values[1], ResourceServerOptions.CryptionKey);

                    if (webToken.Issuer != ResourceServerOptions.Issuer)
                    {
                        await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.IssuersDoNotMatch);
                        return;
                    }

                    DateTime tokenExpiration = webToken.CreatedDate.AddDays(ResourceServerOptions?.TokenDurationInDays ?? 14);

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
                await Next.Invoke(context);
            }
            catch (InvalidOperationException e)
            {
                await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.InsufficientCredentials, e);
                return;
            }
            catch (Exception e)
            {
                await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError, WebApiServerMessages.ServerError, e);
                return;
            }
        }
    }
}
