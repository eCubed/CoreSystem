using CoreLibrary.Cryptography;
using CoreLibrary.NetSecurity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreLibrary.AuthServer
{
    public class TokenIssuerMiddlewareBase<TAuthServerResponse>
        where TAuthServerResponse : IAuthServerResponse, new()
    {

        private RequestDelegate _next;
        private ICrypter _crypter;
        private TokenIssuerOptions _options;

        public TokenIssuerMiddlewareBase(RequestDelegate next,
                                     ICrypter crypter,
                                     TokenIssuerOptions tokenIssuerOptions)
        {
            _next = next;
            _crypter = crypter;
            _options = tokenIssuerOptions;
        }

        protected string GenerateToken(AuthServerRequest authRequest, string issuer, IAdditionalClaimsProvider additionalClaimsProvider)
        {
            WebToken tokenObject = new WebToken();
            tokenObject.Issuer = issuer;

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

            return tokenObject.GenerateToken(_crypter, _options.CryptionKey);
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
                    AuthServerRequest authRequest = CreateAuthServerRequestObject(context.Request);

                    // Now, we have to investigate authRequest
                    if (authRequest.GrantType == "password") // resource owner
                    {
                        if (!(await credentialsProvider.AreUserCredentialsValidAsync(authRequest.Username, authRequest.Password)))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json;charset=utf-8";
                            await context.Response.WriteAsync($"[\"{AuthServerMessages.InvalidUserCredentials}\"]");
                            return;
                        }
                    }
                    else if (authRequest.GrantType == "client") // private client
                    {
                        if (!(await credentialsProvider.AreClientCredentialsValidAsync(authRequest.ClientId, authRequest.ClientSecret)))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json;charset=utf-8";
                            await context.Response.WriteAsync($"[\"{AuthServerMessages.InvalidUserCredentials}\"]");
                            return;
                        }
                    }

                    // Now, we construct the response
                    await WriteResponseAsync(authRequest, _options.Issuer, context.Response, additionalClaimsProvider);
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
