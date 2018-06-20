using FCore.Cryptography;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FCore.Net.Security
{
    public static class TokenTools
    {
        public static string GenerateJwtAccessToken(string issuer, string username, List<string> roles, List<KeyValuePair<string, string>> additionalClaims, string secret)
        {
            JsonWebToken webToken = new JsonWebToken();
            webToken.Issuer = issuer;
            webToken.CreatedDate = DateTime.Now;
            webToken.Claims.Add(new KeyValuePair<string, string>(ClaimTypes.Name, username));

            roles.ForEach(role => webToken.Claims.Add(new KeyValuePair<string, string>(ClaimTypes.Role, role)));

            additionalClaims.ForEach(claimToAdd => webToken.Claims.Add(claimToAdd));

            return webToken.GenerateToken(secret);
        }

        public static string GenerateAccessToken(string issuer, string username, List<string> roles, List<KeyValuePair<string, string>> additionalClaims, ICrypter crypter, string key)
        {
            WebToken webToken = new WebToken(crypter);            
            webToken.Issuer = issuer;
            webToken.CreatedDate = DateTime.Now;
            webToken.Claims.Add(new KeyValuePair<string, string>(ClaimTypes.Name, username));

            roles.ForEach(role => webToken.Claims.Add(new KeyValuePair<string, string>(ClaimTypes.Role, role)));

            additionalClaims.ForEach(claimToAdd => webToken.Claims.Add(claimToAdd));

            return webToken.GenerateToken(key);
        }
    }
}
