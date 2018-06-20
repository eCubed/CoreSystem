using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FCore.Net.Security
{
    public interface IWebToken
    {
        List<KeyValuePair<string, string>> Claims { get; set; }
        string Issuer { get; set; }
        DateTime CreatedDate { get; set; }

        string GenerateToken(string key);
        IWebToken Parse(string stringToken, string key);
        ClaimsPrincipal ConvertToClaimsPrincipal();
    }
}
