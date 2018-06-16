using FCore.Cryptography;
using Newtonsoft.Json;
using System;

namespace FCore.Net.Security
{
    /// <summary>
    /// This is a simplified Jwt
    /// </summary>
    public class WebToken : WebTokenBase
    {
        public override string Issuer { get; set; }
        public override DateTime CreatedDate { get; set; }
        private ICrypter Crypter { get; set; }

        public WebToken() : base()
        {
            CreatedDate = DateTime.Now;
        }

        public WebToken(ICrypter crypter) : this()
        {
            Crypter = crypter;
        }

        public override string GenerateToken(string key)
        {
            return Crypter.Encrypt(JsonConvert.SerializeObject(this), key);
        }

        public override IWebToken Parse(string stringToken, string key)
        {
            string jsonString = Crypter.Decrypt(stringToken, key);

            return JsonConvert.DeserializeObject<WebToken>(jsonString);
        }

        public static WebToken DecryptToken(string tokenString, ICrypter crypter, string key)
        {
            string jsonString = crypter.Decrypt(tokenString, key);

            return JsonConvert.DeserializeObject<WebToken>(jsonString);
        }
    }
}
