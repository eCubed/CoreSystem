using CoreLibrary.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.AuthServer
{
    public class DefaultGrantTypeProcessorFactory : IGrantTypeProcessorFactory
    {
        private ICrypter crypter { get; set; }
        private Dictionary<string, IGrantTypeProcessor> grantTypeProcessors { get; set; }

        public DefaultGrantTypeProcessorFactory(ICrypter crypter, IPasswordCredentialsProvider passwordCredentialsProvider, 
            IPasswordClaimsProvider passwordClaimsProvider, IClientCredentialsProvider clientCredentialsProvider,
            IClientClaimsProvider clientClaimsProvider, IAuthServerResponseProvider<AuthServerResponse> authServerResponseProvider)
        {
            this.crypter = crypter;

            grantTypeProcessors = new Dictionary<string, IGrantTypeProcessor>();

            /* It is here that we manually add GrantTypeProcessors!!!
             */

            // Password.
            grantTypeProcessors.Add("password", new PasswordGrantTypeProcessor<AuthServerResponse>(crypter, "", passwordCredentialsProvider,
                passwordClaimsProvider, authServerResponseProvider));

            // Client

            grantTypeProcessors.Add("client", new ClientGrantTypeProcessor<AuthServerResponse>(crypter, "", clientCredentialsProvider,
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
