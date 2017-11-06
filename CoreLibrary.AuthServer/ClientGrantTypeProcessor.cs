using CoreLibrary.Cryptography;
using Microsoft.AspNetCore.Http;

namespace CoreLibrary.AuthServer
{
    public class ClientGrantTypeProcessor<TAuthServerResponse> : GrantTypeProcessorBase<TAuthServerResponse>
        where TAuthServerResponse : class, IAuthServerResponse, new()
    {
        private IClientCredentialsProvider ClientCredentialsProvider { get; set; }

        public ClientGrantTypeProcessor(ICrypter crypter, string cryptionKey,
            IClientCredentialsProvider passwordCredentialsProvider, IClientClaimsProvider passwordClaimsProvider,
            IAuthServerResponseProvider<TAuthServerResponse> authServerResponseProvider)
            : base(crypter, cryptionKey, passwordCredentialsProvider, passwordClaimsProvider, authServerResponseProvider)
        {
            ClientCredentialsProvider = passwordCredentialsProvider;
        }

        protected override string InvalidCredentialsMessage => AuthServerMessages.InvalidClientCredentials;

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
            return request.Form["client_secret"].ToString();
        }

        protected override string ExtractUniqueIdentifier(HttpRequest request)
        {
            return request.Form["client_id"].ToString();
        }

    }
}
