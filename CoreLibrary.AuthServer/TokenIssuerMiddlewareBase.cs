using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CoreLibrary.AuthServer
{
    public class TokenIssuerMiddlewareBase<TAuthServerResponse>
        where TAuthServerResponse : class, IAuthServerResponse, new()
    {
        private RequestDelegate _next;
        private TokenIssuerOptions _options;

        public TokenIssuerMiddlewareBase(RequestDelegate next,
                                     TokenIssuerOptions tokenIssuerOptions)
        {
            _next = next;
            _options = tokenIssuerOptions;
        }
       
        public async Task Invoke(HttpContext context, IGrantTypeProcessorFactory<TAuthServerResponse> grantTypeProcessorFactory)
        {
            if (context.Request.Path.Value.Equals(_options.TokenEndpointPath))
            {
                if (context.Request.ContentType != "application/x-www-form-urlencoded")
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    await context.Response.WriteAsync("[\"invalid-content-type\"]");
                    return;
                }

                try
                {
                    IGrantTypeProcessor grantTypeProcessor = grantTypeProcessorFactory.CreateInstance(context.Request.Form["grant_type"].ToString() ?? "", _options.CryptionKey);

                    var res = await grantTypeProcessor.ProcessCredentialsAsync(context.Request, context.Response);

                    if (!res.Success)
                        return;
                    
                    await grantTypeProcessor.WriteTokenResponseAsync(context.Response, context.Request, _options.Issuer);
                    return;
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json;charset=utf-8";
                    await context.Response.WriteAsync($"[\"{e.Message}\"]");
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
