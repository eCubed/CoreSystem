using FCore.WebApiServerBase;
using Microsoft.AspNetCore.Http;
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

        public async Task Invoke(HttpContext context, IGrantTypeRepository grantTypeRepository)
        {
            if (context.Request.Path.Value.Equals(TokenIssuerOptions.TokenEndpointPath))
            {
                if (context.Request.ContentType != "application/x-www-form-urlencoded")
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, AuthServerMessages.InvalidContentType);
                    return;
                }

                GrantType grantType = grantTypeRepository.GetGrantType(context.Request.Form["grant_type"]);

                if (grantType == null)
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, AuthServerMessages.InvalidGrantType);
                    return;
                }

                // Make sure that the required parameters are present, per grantType. No actual user/client/etc authentication happened at this step.
                var validateTokenRes = grantType.ValidateTokenRequest(context.Request);

                if (!validateTokenRes.Success)
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, validateTokenRes.Errors.ToList().First());
                
                /* We need to extract the values from the form based on the grant type's parameters.
                 */
            }


            await Next.Invoke(context);
        }
    }
}
