using CoreLibrary.Cryptography;
using CoreLibrary.NetSecurity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLibrary.AuthServer
{
    public abstract class GrantTypeProcessorBase : IGrantTypeProcessor
    {
        private ICrypter crypter { get; set; }
        private string cryptionKey { get; set; }

        public GrantTypeProcessorBase(ICrypter crypter, string cryptionKey)
        {
            this.crypter = crypter;
            this.cryptionKey = cryptionKey;
        }

        protected abstract string InvalidCredentialsMessage { get; set; }

        protected abstract bool AreCredentialsValid(string identifier, string passCode);

        protected abstract IAuthServerRequest CreateAuthServerRequest(HttpRequest httpRequest);

        protected abstract List<KeyValuePair<string, string>> GetAdditionalClaims(string uniqueIdentifier);

        protected abstract string ExtractUniqueIdentifier(HttpRequest request);

        protected abstract string ExtractPasscode(HttpRequest request);

        protected abstract void AddSpecificGrantTypeClaims(WebToken token);

        protected abstract IAuthServerResponse CreateNewAuthServerResponse();

        protected abstract void SetOtherAuthServerResponseProperties(string uniqueIdentifier, IAuthServerResponse authServerResponse);

        private string GenerateToken(HttpRequest request, string issuer)
        {
            WebToken token = new WebToken();
            token.Issuer = issuer;

            AddSpecificGrantTypeClaims(token);

            List<KeyValuePair<string, string>> additionalClaims = GetAdditionalClaims(ExtractUniqueIdentifier(request));
                        
            if ((additionalClaims != null) && (additionalClaims.Count > 0))
            {
                foreach (var kvp in additionalClaims)
                {
                    token.AddClaim(kvp.Key, kvp.Value);
                }
            }

            return token.GenerateToken(crypter, cryptionKey);
        }


        public async Task<ManagerResult> ProcessHttpRequestAsync(HttpRequest request, HttpResponse response)
        {
            if (! AreCredentialsValid(ExtractUniqueIdentifier(request), ExtractPasscode(request)))
            {
                response.StatusCode = StatusCodes.Status401Unauthorized;
                response.ContentType = "application/json;charset=utf-8";
                await response.WriteAsync($"[\"{InvalidCredentialsMessage}\"]");

                return new ManagerResult(InvalidCredentialsMessage);
            }

            return new ManagerResult();
        }

        public async Task WriteToHttpResponseAsync(HttpResponse response, HttpRequest request, string issuer)
        {
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/json;charset=utf-8";

            IAuthServerResponse newAuthServerResponse = CreateNewAuthServerResponse();
            newAuthServerResponse.AccessToken = GenerateToken(request, issuer);
            SetOtherAuthServerResponseProperties(ExtractUniqueIdentifier(request), newAuthServerResponse);
            await response.WriteAsync(JsonConvert.SerializeObject("authServerResponse"));
            /*
            TAuthServerResponse authServerResponse = new TAuthServerResponse();
            authServerResponse.AccessToken = GenerateToken(authRequest, issuer, additionalClaimsProvider);
            SetOtherAuthServerResponseProperties(authRequest, authServerResponse);
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/json;charset=utf-8";
            await response.WriteAsync(JsonConvert.SerializeObject(authServerResponse));
            */
        }

    }
}
