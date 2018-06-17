using FCore.Net.Security;
using FCore.WebApiServerBase;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FCore.AuthServer
{
    public abstract class TokenIssuerMiddlewareBase<TAuthServerResponse, TWebToken>
        where TAuthServerResponse : IAuthServerResponse, new()
        where TWebToken : class, IWebToken
    {
        private RequestDelegate Next;
        private TokenIssuerOptions TokenIssuerOptions;

        public TokenIssuerMiddlewareBase(RequestDelegate next,
                                     TokenIssuerOptions tokenIssuerOptions)
        {
            Next = next;
            TokenIssuerOptions = tokenIssuerOptions;
        }

        protected abstract IWebToken CreateStarterTokenInstance();

        protected string GenerateToken(AuthServerRequest authRequest, string issuer, IAdditionalClaimsProvider additionalClaimsProvider)
        {
            WebToken tokenObject = new WebToken();
            tokenObject.Issuer = issuer;
            tokenObject.CreatedDate = DateTime.Now;

            List<KeyValuePair<string, string>> additionalClaims = null;

            if (authRequest.GrantType == "password")
            {
                tokenObject.AddClaim(ClaimTypes.Name, authRequest.Username);
                if (!string.IsNullOrEmpty(authRequest.ClientId))
                    tokenObject.AddClaim("ClientId", authRequest.ClientId);

                additionalClaims = additionalClaimsProvider.GetAdditionalUserClaims(authRequest.Username);
            }
            else if (authRequest.GrantType == "client")
            {
                tokenObject.AddClaim("ClientId", authRequest.ClientId);

                additionalClaims = additionalClaimsProvider.GetAdditionalClientClaims(authRequest.ClientId);
            }

            if ((additionalClaims != null) && (additionalClaims.Count > 0))
            {
                foreach (var kvp in additionalClaims)
                {
                    tokenObject.AddClaim(kvp.Key, kvp.Value);
                }
            }

            return tokenObject.GenerateToken(TokenIssuerOptions.CryptionKey);
        }

        protected virtual void SetOtherAuthServerResponseProperties(AuthServerRequest authServerRequest, TAuthServerResponse authServerResponse)
        {
        }

        protected async Task WriteResponseAsync(AuthServerRequest authRequest, string issuer, HttpResponse response, IAdditionalClaimsProvider additionalClaimsProvider)
        {
            TAuthServerResponse authServerResponse = new TAuthServerResponse();
            authServerResponse.AccessToken = GenerateToken(authRequest, issuer, additionalClaimsProvider);
            SetOtherAuthServerResponseProperties(authRequest, authServerResponse);
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/json;charset=utf-8";
            await response.WriteAsync(JsonConvert.SerializeObject(authServerResponse));
        }

        protected AuthServerRequest CreateAuthServerRequestObject(HttpRequest httpRequest)
        {
            AuthServerRequest request = new AuthServerRequest();
            request.ClientId = httpRequest.Form["client_id"].ToString() ?? "";
            request.ClientSecret = httpRequest.Form["client_secret"].ToString() ?? "";
            request.GrantType = httpRequest.Form["grant_type"].ToString() ?? "";
            request.Username = httpRequest.Form["username"].ToString() ?? "";
            request.Password = httpRequest.Form["password"].ToString() ?? "";

            return request;
        }

        public async Task Invoke(HttpContext context, ICredentialsProvider credentialsProvider, IAdditionalClaimsProvider additionalClaimsProvider)
        {
            if (context.Request.Path.Value.Equals(TokenIssuerOptions.TokenEndpointPath))
            {
                if (context.Request.ContentType != "application/x-www-form-urlencoded")
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, AuthServerMessages.InvalidContentType);
                    return;
                }

                try
                {
                    AuthServerRequest authRequest = CreateAuthServerRequestObject(context.Request);

                    // Now, we have to investigate authRequest
                    if (authRequest.GrantType == "password") // resource owner
                    {
                        if (!(await credentialsProvider.AreUserCredentialsValidAsync(authRequest.Username, authRequest.Password)))
                        {
                            await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, AuthServerMessages.InvalidUserCredentials);
                            return;
                        }
                    }
                    else if (authRequest.GrantType == "client") // private client
                    {
                        if (!(await credentialsProvider.AreClientCredentialsValidAsync(authRequest.ClientId, authRequest.ClientSecret)))
                        {                          
                            await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, AuthServerMessages.InvalidUserCredentials);
                            return;
                        }
                    }

                    // Now, we construct the response
                    await WriteResponseAsync(authRequest, TokenIssuerOptions.Issuer, context.Response, additionalClaimsProvider);
                    return;

                }
                catch (Exception e)
                {
                    await WebApiMiddlewareHelpers.WriteErrorResponseAsync(context, StatusCodes.Status400BadRequest, WebApiServerMessages.ServerError, e);
                    return;
                }
            }

            await Next.Invoke(context);
        }
    }
}
