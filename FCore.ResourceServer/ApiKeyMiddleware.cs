using FCore.WebApiServerBase;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FCore.ResourceServer
{
    public class ApiKeyMiddleware<TApiClient>
        where TApiClient : class, IApiClient
    {
        private RequestDelegate _next;
        private ApiKeyMiddlewareOptions ApiKeyMiddlewareOptions;

        public ApiKeyMiddleware(RequestDelegate next, ApiKeyMiddlewareOptions apiKeyMiddlewareOptions = null)
        {
            _next = next;
            ApiKeyMiddlewareOptions = apiKeyMiddlewareOptions ?? new ApiKeyMiddlewareOptions();
        }

        private ClaimsPrincipal CreateClaimsPrincipalForClient(string clientIdString)
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ApiKeyMiddlewareOptions.ClientIdentifierKey, clientIdString));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            principal.AddIdentity(claimsIdentity);

            return principal;
        }

        public async Task Invoke(HttpContext context, IApiClientProvider<TApiClient> apiClientProvider, IApiClientHasher apiClientHasher)
        {
            if (context.Request.Headers.ContainsKey(ApiKeyMiddlewareOptions.ApiKeyHeaderKey))
            {
                if (!context.Request.Headers.ContainsKey(ApiKeyMiddlewareOptions.HashHeaderKey))
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.HashHeaderRequired);
                    return;
                }

                if (!context.Request.Headers.ContainsKey(ApiKeyMiddlewareOptions.DataHeaderKey))
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.DataHeaderRequired);
                    return;
                }

                string apiKey = context.Request.Headers[ApiKeyMiddlewareOptions.ApiKeyHeaderKey].ToString();

                if (!apiClientProvider.ClientExists(apiKey))
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.InvalidClient);
                    return;
                }

                TApiClient apiClient = apiClientProvider.GetClientByPublicKey(apiKey);

                string[] clientAuthValues = context.Request.Headers[ApiKeyMiddlewareOptions.HashHeaderKey].ToString().Split(' ');

                if (clientAuthValues.Length != 1)
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, ResourceServerMessages.HashHeaderRequired);
                    return;
                }

                string clientHash = clientAuthValues[0];
                string clientSecret = apiClientProvider.GetClientSecret(apiKey);

                string xInputValue = context.Request.Headers[ApiKeyMiddlewareOptions.DataHeaderKey].ToString();

                if (!apiClientHasher.CheckHash(apiKey, clientSecret, xInputValue, clientHash))
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, ResourceServerMessages.InvalidHash);
                    return;
                }

                // Now that the headers validate, we now need to store that client id into the principal...
                context.User = CreateClaimsPrincipalForClient(apiClient.GetIdAsString());
            }

            await _next.Invoke(context);
        }

    }
}
