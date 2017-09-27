using System.Collections.Generic;

namespace CoreLibrary.AuthServer
{
    public class TokenIssuerOptions
    {
        /// <summary>
        /// The encryption/decryption key for token generation and decrypting.
        /// It must be a string of a multiple of 16 characters, alpha-numeric, and
        /// may include speial symbols.
        /// </summary>
        public string CryptionKey { get; set; }

        public string TokenEndpointPath { get; set; }

        /// <summary>
        /// The issuer would have to identify itself because the resource server MUST match
        /// the issuer (that is, it should know from where it's being authorized.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// A list of the resource servers that may use the generated token
        /// </summary>
        public List<string> Audiences { get; set; }

        public TokenIssuerOptions()
        {
            TokenEndpointPath = "/token";            
        }
    }
}
