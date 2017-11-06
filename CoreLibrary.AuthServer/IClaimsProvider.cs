using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CoreLibrary.AuthServer
{
    public interface IClaimsProvider
    {
        List<KeyValuePair<string, string>> GetBasicClaims(string uniqueIdentifier, HttpRequest request);
        List<KeyValuePair<string, string>> GetAdditionalClaims(string uniqueIdentifier);
    }
}
