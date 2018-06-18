using FCore.Net.Security;
using FCore.WebApiServerBase;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.AuthServer
{
    public class NewTokenIssuerMiddleware
    {
        private RequestDelegate Next;
        private TokenIssuerOptions TokenIssuerOptions;

        public NewTokenIssuerMiddleware(RequestDelegate next,
            TokenIssuerOptions tokenIssuerOptions)
        {
            Next = next;
            TokenIssuerOptions = tokenIssuerOptions;
        }

        private async Task WriteResponseAsync(IAuthServerResponse authServerResponse, HttpResponse response)
        {
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/json;charset=utf-8";
            await response.WriteAsync(JsonConvert.SerializeObject(authServerResponse));
        }

        public async Task Invoke(HttpContext context, IGrantTypeProcessorFactory grantTypeProcessorFactory)
        {
            if (context.Request.Path.Value.Equals(TokenIssuerOptions.TokenEndpointPath))
            {
                if (context.Request.ContentType != "application/x-www-form-urlencoded")
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, AuthServerMessages.InvalidContentType);
                    return;
                }

                IGrantTypeProcessor grantTypeProcessor = grantTypeProcessorFactory.CreateInstance(context.Request.Form["grant_type"]);

                if (grantTypeProcessor == null)
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, AuthServerMessages.InvalidGrantType);
                    return;
                }

                // Make sure that the required parameters are present, per grantType. No actual user/client/etc authentication happened at this step.
                var validateTokenRes = grantTypeProcessor.ValidateRequest(context.Request);

                if (!validateTokenRes.Success)
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, validateTokenRes.Errors.ToList().First());

                // Validate the identifier
                var validateIdentifierRes = grantTypeProcessor.ValidateIdentifier(context.Request.Form[grantTypeProcessor.IdentifierName],
                    context.Request.Form[grantTypeProcessor.PasscodeName]);

                if (!validateIdentifierRes.Success)
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, validateIdentifierRes.Errors.ToList().First());

                // Obtain Additional Claims
                List<KeyValuePair<string, string>> additionalClaims = grantTypeProcessor.ObtainAdditionalClaims(context.Request.Form[grantTypeProcessor.IdentifierName]);

                // Now, formulate the token object from the identifier value and the additional claims.
                IWebToken token = grantTypeProcessor.GenerateWebTokenObject(context.Request.Form[grantTypeProcessor.IdentifierName], additionalClaims);

                string accessToken = token.GenerateToken(TokenIssuerOptions.CryptionKey);

                // Now, assemble the Response object... Won't it be different per grant type!?
                IAuthServerResponse authServerResponse = grantTypeProcessor.GenerateAuthServerResponse(accessToken, context.Request.Form[grantTypeProcessor.IdentifierName]);

                // Finally, we write the response.
                await WriteResponseAsync(authServerResponse, context.Response);
            }

            await Next.Invoke(context);
        }
    }
}
