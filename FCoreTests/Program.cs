using FCore.Cryptography;
using FCore.Net.Security;
using System;
using System.Security.Claims;

namespace FCoreTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            
            JsonWebToken token = new JsonWebToken();
            
            token.Issuer = "FCore Test Issuer";
            token.CreatedDate = DateTime.Now;
            token.AddClaim(ClaimTypes.Name, "joe");
            token.AddClaim(ClaimTypes.Role, "member");

            string tokenString = token.GenerateToken("test-test-test--");
            
            /*
            WebToken token = new WebToken(new Crypt());
            token.Issuer = "FCore Test Issuer";
            token.CreatedDate = DateTime.Now;
            token.AddClaim(ClaimTypes.Name, "joe");
            token.AddClaim(ClaimTypes.Role, "member");
            
            string tokenString = token.GenerateToken("test-test-test--");
            */
            Console.WriteLine(tokenString);
            
            
            Console.ReadLine();
        }
    }
}
