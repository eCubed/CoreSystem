using CoreLibrary.Cryptography;
using System;
using System.Collections.Generic;

namespace CoreLibrary.AuthServer
{
    public class DefaultGrantTypeProcessorFactory<TAuthServerResponse> : IGrantTypeProcessorFactory<TAuthServerResponse>
        where TAuthServerResponse : class, IAuthServerResponse,  new()
    {
        private ICrypter crypter { get; set; }
        private Dictionary<string, IGrantTypeProcessor> grantTypeProcessors { get; set; }

        public DefaultGrantTypeProcessorFactory(ICrypter crypter, IPasswordCredentialsProvider passwordCredentialsProvider, 
            IPasswordClaimsProvider passwordClaimsProvider, IClientCredentialsProvider clientCredentialsProvider,
            IClientClaimsProvider clientClaimsProvider, IAuthServerResponseProvider<TAuthServerResponse> authServerResponseProvider)
        {
            this.crypter = crypter;

            grantTypeProcessors = new Dictionary<string, IGrantTypeProcessor>();

            /* It is here that we manually add GrantTypeProcessors!!!
             */

            // Password.
            grantTypeProcessors.Add("password", new PasswordGrantTypeProcessor<TAuthServerResponse>(crypter, "", passwordCredentialsProvider,
                passwordClaimsProvider,  authServerResponseProvider));
            
            // Client

            grantTypeProcessors.Add("client", new ClientGrantTypeProcessor<TAuthServerResponse>(crypter, "", clientCredentialsProvider,
                clientClaimsProvider, authServerResponseProvider));
        }

        public IGrantTypeProcessor CreateInstance(string grantType, string cryptionKey)
        {
            if (!grantTypeProcessors.ContainsKey(grantType))
                throw new Exception(AuthServerMessages.InvalidGrantType);

            return grantTypeProcessors.GetValueOrDefault(grantType);
        }
    }
}
