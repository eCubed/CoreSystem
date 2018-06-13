using System.Collections.Generic;

namespace FCore.AuthServer
{
    public interface IAdditionalClaimsProvider
    {
        List<KeyValuePair<string, string>> GetAdditionalUserClaims(string username);
        List<KeyValuePair<string, string>> GetAdditionalClientClaims(string clientId);
    }
}
