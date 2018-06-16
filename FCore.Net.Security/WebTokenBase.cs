using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FCore.Net.Security
{
    public abstract class WebTokenBase : IWebToken
    {
        public List<KeyValuePair<string, string>> Claims { get; set; }
        public abstract string Issuer { get; set; }
        public abstract DateTime CreatedDate { get; set; }

        public abstract string GenerateToken(string key);

        public abstract IWebToken Parse(string stringToken, string key);

        public WebTokenBase()
        {
            Claims = new List<KeyValuePair<string, string>>();
        }

        public void AddClaim(string claimType, string value)
        {
            Claims.Add(new KeyValuePair<string, string>(claimType, value));
        }

        public ClaimsPrincipal ConvertToClaimsPrincipal()
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();

            List<Claim> claims = new List<Claim>();

            Claims.ToList().ForEach(kvp =>
            {
                claims.Add(new Claim(kvp.Key, kvp.Value));
            });

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

            principal.AddIdentity(claimsIdentity);

            return principal;
        }
    }
}
