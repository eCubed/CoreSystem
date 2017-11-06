using CoreLibrary.Cryptography;
using CoreLibrary.NetSecurity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLibrary.AuthServer
{
    public abstract class GrantTypeProcessorBase : IGrantTypeProcessor
    {
        private ICrypter Crypter { get; set; }
        private string CryptionKey { get; set; }
        private ICredentialsProvider CredentialsProvider { get; set; }
        private IClaimsProvider ClaimsProvider { get; set; }

        public GrantTypeProcessorBase(ICrypter crypter, string cryptionKey, ICredentialsProvider credentialsProvider,
            IClaimsProvider claimsProvider)
        {
            Crypter = crypter;
            CryptionKey = cryptionKey;
            CredentialsProvider = credentialsProvider;
            ClaimsProvider = claimsProvider;
        }

        protected abstract string InvalidCredentialsMessage { get; }
        
        protected abstract string ExtractUniqueIdentifier(HttpRequest request);

        protected abstract string ExtractPasscode(HttpRequest request);
        
        protected abstract IAuthServerResponse CreateNewAuthServerResponse();

        protected abstract void SetOtherAuthServerResponseProperties(string uniqueIdentifier, IAuthServerResponse authServerResponse);

        private void LoadClaimsToToken(WebToken token, List<KeyValuePair<string, string>> claims)
        {
            if ((claims != null) && (claims.Count > 0))
            {
                foreach (var kvp in claims)
                {
                    token.AddClaim(kvp.Key, kvp.Value);
                }
            }
        }

        private string GenerateToken(HttpRequest request, string issuer)
        {
            WebToken token = new WebToken();
            token.Issuer = issuer;

            LoadClaimsToToken(token, ClaimsProvider.GetBasicClaims(ExtractUniqueIdentifier(request), request));
            LoadClaimsToToken(token, ClaimsProvider.GetAdditionalClaims(ExtractUniqueIdentifier(request)));           

            return token.GenerateToken(Crypter, CryptionKey);
        }

        public async Task<ManagerResult> ProcessCredentialsAsync(HttpRequest request, HttpResponse response)
        {
            if (! (await CredentialsProvider.AreCredentialsValidAsync(ExtractUniqueIdentifier(request), ExtractPasscode(request))))
            {
                response.StatusCode = StatusCodes.Status401Unauthorized;
                response.ContentType = "application/json;charset=utf-8";
                await response.WriteAsync($"[\"{InvalidCredentialsMessage}\"]");

                return new ManagerResult(InvalidCredentialsMessage);
            }

            return new ManagerResult();
        }

        public async Task WriteTokenResponseAsync(HttpResponse response, HttpRequest request, string issuer)
        {
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/json;charset=utf-8";

            IAuthServerResponse newAuthServerResponse = CreateNewAuthServerResponse();
            newAuthServerResponse.AccessToken = GenerateToken(request, issuer);
            SetOtherAuthServerResponseProperties(ExtractUniqueIdentifier(request), newAuthServerResponse);
            await response.WriteAsync(JsonConvert.SerializeObject(newAuthServerResponse));
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
