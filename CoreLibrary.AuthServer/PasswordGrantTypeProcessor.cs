using CoreLibrary.Cryptography;
using Microsoft.AspNetCore.Http;
using System;

namespace CoreLibrary.AuthServer
{
    public class PasswordGrantTypeProcessor<TAuthServerResponse> : GrantTypeProcessorBase<TAuthServerResponse>
        where TAuthServerResponse : class, IAuthServerResponse, new()
    {
        private IPasswordCredentialsProvider PasswordCredentialsProvider { get; set; }

        public PasswordGrantTypeProcessor(ICrypter crypter, string cryptionKey,
            IPasswordCredentialsProvider passwordCredentialsProvider, IPasswordClaimsProvider passwordClaimsProvider,
            IAuthServerResponseProvider<TAuthServerResponse> authServerResponseProvider) 
            : base(crypter, cryptionKey, passwordCredentialsProvider, passwordClaimsProvider, authServerResponseProvider)
        {
            PasswordCredentialsProvider = passwordCredentialsProvider;
        }

        protected override string InvalidCredentialsMessage => AuthServerMessages.InvalidUserCredentials;

        /*
        protected override void AddSpecificGrantTypeClaims(WebToken token, HttpRequest request)
        {
            token.AddClaim(ClaimTypes.Name, ExtractUniqueIdentifier(request));
            if (!string.IsNullOrEmpty(request.Form["client_id"]))
                token.AddClaim("ClientId", request.Form["client_id"].ToString());
        }
        */
    
        protected override string ExtractPasscode(HttpRequest request)
        {
            return request.Form["password"].ToString();
        }

        protected override string ExtractUniqueIdentifier(HttpRequest request)
        {
            return request.Form["username"].ToString();
        }
        
    }
}
