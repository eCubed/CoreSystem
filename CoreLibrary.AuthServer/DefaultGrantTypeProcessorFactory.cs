using CoreLibrary.Cryptography;
using System;

namespace CoreLibrary.AuthServer
{
    public abstract class DefaultGrantTypeProcessorFactory<TAuthServerResponse> : IGrantTypeProcessorFactory<TAuthServerResponse>
        where TAuthServerResponse : class, IAuthServerResponse,  new()
    {
        private ICrypter crypter { get; set; }
        private IPasswordCredentialsProvider PasswordCredentialsProvider { get; set; }
        private IPasswordClaimsProvider PasswordClaimsProvider { get; set; }
        private IClientCredentialsProvider ClientCredentialsProvider { get; set; }
        private IClientClaimsProvider ClientClaimsProvider { get; set; }
        private IAuthServerResponseProvider<TAuthServerResponse> AuthServerResponseProvider { get; set; }

        public DefaultGrantTypeProcessorFactory(ICrypter crypter, IPasswordCredentialsProvider passwordCredentialsProvider, 
            IPasswordClaimsProvider passwordClaimsProvider, IClientCredentialsProvider clientCredentialsProvider,
            IClientClaimsProvider clientClaimsProvider, IAuthServerResponseProvider<TAuthServerResponse> authServerResponseProvider)
        {
            this.crypter = crypter;
            PasswordCredentialsProvider = passwordCredentialsProvider;
            PasswordClaimsProvider = passwordClaimsProvider;
            ClientCredentialsProvider = clientCredentialsProvider;
            ClientClaimsProvider = clientClaimsProvider;
            AuthServerResponseProvider = authServerResponseProvider;            
        }

        protected virtual IGrantTypeProcessor CreateAdditionalGrantTypeProcessorInstances(string grantType, string cryptionKey)
        {
            throw new Exception(AuthServerMessages.InvalidGrantType);
        }

        public IGrantTypeProcessor CreateInstance(string grantType, string cryptionKey)
        {
            if (grantType == GrantTypeNames.Password)
                return new PasswordGrantTypeProcessor<TAuthServerResponse>(crypter, cryptionKey, PasswordCredentialsProvider,
                    PasswordClaimsProvider, AuthServerResponseProvider);
            else if (grantType == GrantTypeNames.Client)
                return new ClientGrantTypeProcessor<TAuthServerResponse>(crypter, cryptionKey, ClientCredentialsProvider,
                    ClientClaimsProvider, AuthServerResponseProvider);
            else
                return CreateAdditionalGrantTypeProcessorInstances(grantType, cryptionKey);

            throw new Exception(AuthServerMessages.InvalidGrantType);
        }
    }
}
