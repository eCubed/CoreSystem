using CoreLibrary.Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CoreLibrary.NetSecurity
{
    /// <summary>
    /// This is a simplified Jwt
    /// </summary>
    public class WebToken
    {
        public List<KeyValuePair<string, string>> Claims { get; set; }
        public string Issuer { get; set; }
        public DateTime CreatedDate { get; set; }

        public WebToken()
        {
            Claims = new List<KeyValuePair<string, string>>();
            CreatedDate = DateTime.Now;
        }

        public string GenerateToken(ICrypter crypter, string key)
        {
            return crypter.Encrypt(JsonConvert.SerializeObject(this), key);
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

        public static WebToken DecryptToken(string tokenString, ICrypter crypter, string key)
        {
            string jsonString = crypter.Decrypt(tokenString, key);

            return JsonConvert.DeserializeObject<WebToken>(jsonString);
        }
    }
}
