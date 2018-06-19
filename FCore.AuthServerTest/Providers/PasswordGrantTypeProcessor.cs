using FCore.AuthServer;
using System;
using System.Collections.Generic;
using FCore.Foundations;
using FCore.Net.Security;
using System.Security.Claims;

namespace FCore.AuthServerTest.Providers
{
    public class PasswordGrantTypeProcessor : PasswordGrantTypeProcessorBase
    {
        private List<string> Roles { get; set; }
        
        public PasswordGrantTypeProcessor() : base()
        {
            Roles = new List<string> { "admin", "moderator", "member" };
        }

        public override IAuthServerResponse GenerateAuthServerResponse(string accessToken, string identifierValue)
        {
            return new DefaultAuthServerResponse { AccessToken = accessToken, Roles = this.Roles };
        }
        
        public override IWebToken GenerateWebTokenObject(TokenIssuerOptions tokenIssuerOptions, string identifierValue, List<KeyValuePair<string, string>> additionalClaims)
        {
            JsonWebToken jwt = new JsonWebToken();
            jwt.Issuer = tokenIssuerOptions.Issuer;
            jwt.CreatedDate = DateTime.Now;

            jwt.AddClaim(ClaimTypes.Name, identifierValue);

            Roles.ForEach(role =>
            {
                jwt.AddClaim(ClaimTypes.Role, role);
            });

            additionalClaims.ForEach(claimKvp =>
            {
                jwt.AddClaim(claimKvp.Key, claimKvp.Value);
            });

            // We won't do

            return jwt;            
        }

        public override List<KeyValuePair<string, string>> ObtainAdditionalClaims(string identifierValue)
        {
            List<KeyValuePair<string, string>> additionalClaims = new List<KeyValuePair<string, string>>();

            additionalClaims.Add(new KeyValuePair<string, string>("Favorite Movie", "Jurassic Park"));
            additionalClaims.Add(new KeyValuePair<string, string>(ClaimTypes.PostalCode, "52806"));

            return additionalClaims;
        }

        public override ManagerResult ValidateIdentifier(string identifierValue, string passcodeValue)
        {
            return new ManagerResult();
        }
    }
}
