using FCore.Cryptography;
using FCore.Foundations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FCore.Net.Security
{
    public class JsonWebToken
    {
        public List<KeyValuePair<string, string>> Claims { get; set; }

        public JsonWebToken()
        {
            Claims = new List<KeyValuePair<string, string>>();
        }

        [JsonIgnore]
        public string Issuer
        {
            set
            {
                if (Claims == null)
                    Claims = new List<KeyValuePair<string, string>>();

                Claims.Add(new KeyValuePair<string, string>("Issuer", value));
            }

            get
            {
                if (Claims.Any(c => c.Key == "Issuer"))
                {
                    return Claims.FirstOrDefault(c => c.Key == "Issuer").Value;
                }

                return "";
            }
        }

        [JsonIgnore]
        public DateTime CreatedDate
        {
            set
            {
                if (Claims == null)
                    Claims = new List<KeyValuePair<string, string>>();

                Claims.Add(new KeyValuePair<string, string>("CreatedDate", value.ToLongDateString()));
            }

            get
            {
                if (Claims.Any(c => c.Key == "CreatedDate"))
                {
                    return Convert.ToDateTime(Claims.FirstOrDefault(c => c.Key == "CreatedDate").Value);
                }

                return DateTime.Now;
            }
        }

        public string GenerateToken(string secret)
        {
            string header = (new JsonWebTokenHeader() { Algorithm = "HS256", Type = "JWT" }).GetJsonSerialization();
            string payload = JsonConvert.SerializeObject(this);
            string encodedPortion = $"{EncodingTools.ToBase64String(header)}.{EncodingTools.ToBase64String(payload)}";
            string signature = Hasher.GetHash(encodedPortion + secret, Hasher.HashType.SHA256);

            return $"{encodedPortion}.{signature}";
        }

        public static JsonWebToken Parse(string jwt, string secret)
        {
            string headerJson = "";
            string payloadJson = "";
            string signature = "";
            string encodedPortion = "";
            string[] jwtSplit = null;

            try
            {
                jwtSplit = jwt.Split('.');
            }
            catch (Exception e)
            {
                throw e;
            }

            encodedPortion = $"{jwtSplit[0]}.{jwtSplit[1]}";
            string expectedSignature = Hasher.GetHash(encodedPortion + secret, Hasher.HashType.SHA256);

            if (!expectedSignature.Equals(jwtSplit[2]))
                throw new Exception("Invalid token");

            headerJson = EncodingTools.FromBase64String(jwtSplit[0]);
            payloadJson = EncodingTools.FromBase64String(jwtSplit[1]);
            signature = jwtSplit[2];

            return JsonConvert.DeserializeObject<JsonWebToken>(payloadJson);
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
