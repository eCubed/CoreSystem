using CoreLibrary.Cryptography;
using CoreLibrary.NetSecurity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace CoreLibrary.AuthServer
{
    public class PasswordGrantTypeProcessor : GrantTypeProcessorBase
    {
        private IPasswordCredentialsProvider PasswordCredentialsProvider { get; set; }

        public PasswordGrantTypeProcessor(ICrypter crypter, string cryptionKey,
            IPasswordCredentialsProvider passwordCredentialsProvider, IClaimsProvider claimsProvider) 
            : base(crypter, cryptionKey, passwordCredentialsProvider, claimsProvider)
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
                
        protected override IAuthServerResponse CreateNewAuthServerResponse()
        {
            throw new NotImplementedException();
        }

        protected override string ExtractPasscode(HttpRequest request)
        {
            return request.Form["password"].ToString();
        }

        protected override string ExtractUniqueIdentifier(HttpRequest request)
        {
            return request.Form["username"].ToString();
        }

        protected override void SetOtherAuthServerResponseProperties(string uniqueIdentifier, IAuthServerResponse authServerResponse)
        {
            throw new NotImplementedException();
        }
    }
}
