using FCore.AuthServer;
using FCore.Foundations;
using FCore.Net.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FCore.AuthServerTest.Providers
{
    public class PortalGrantTypeProcessor : PortalGrantTypeProcessorBase
    {
        public PortalGrantTypeProcessor() : base()
        {
        }

        public override IAuthServerResponse GenerateAuthServerResponse(string accessToken, string identifierValue)
        {
            return new PortalAuthServerResponse
            {
                AccessToken = accessToken,
                Roles = new List<string> { "admin", "member" },
                Portals = new List<string> { "Rebel Base", "Naboo" }
            };
        }

        public override IWebToken GenerateWebTokenObject(TokenIssuerOptions tokenIssuerOptions, string identifierValue, List<KeyValuePair<string, string>> additionalClaims)
        {
            JsonWebToken jwt = new JsonWebToken();
            jwt.Issuer = tokenIssuerOptions.Issuer;
            jwt.CreatedDate = DateTime.Now;
            jwt.AddClaim(ClaimTypes.Name, identifierValue);
            jwt.AddClaim(ClaimTypes.Role, "admin");
            jwt.AddClaim(ClaimTypes.Role, "member");

            additionalClaims.ForEach(claimKvp =>
            {
                jwt.AddClaim(claimKvp.Key, claimKvp.Value);
            });

            return jwt;
        }

        public override List<KeyValuePair<string, string>> ObtainAdditionalClaims(string identifierValue)
        {
            List<KeyValuePair<string, string>> stuff = new List<KeyValuePair<string, string>>();
            stuff.Add(new KeyValuePair<string, string>("Portal", "Rebel Base"));

            return stuff;
        }

        public override ManagerResult ValidateIdentifier(string identifierValue, string passcodeValue, List<KeyValuePair<string, string>> otherRequiredParamValues)
        {
            //List<KeyValuePair<string, string>> extraParamValues = GetOtherRequiredParametersFromRequest()
            string portalName = otherRequiredParamValues.SingleOrDefault(p => p.Key == "portal").Value;

            if (portalName == "Death Star")
                return new ManagerResult("invalid-portal");

            return new ManagerResult();
        }
    }
}
