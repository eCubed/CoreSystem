using System.Collections.Generic;
using System.Linq;

namespace FCore.AuthServer
{
    /// <summary>
    /// The default GrantTypeRepository will include password, client, and portal grant types. This class may
    /// be inherited to add more grant types. Inheriting to add more grant types to the list seems like a nonsense
    /// move, but we need to register a repository pre-filled at application startup via DI, so we must
    /// subclass if we wish to add more than just password and client grant types.
    /// </summary>
    public class DefaultGrantTypeRepository : IGrantTypeRepository
    {
        public List<GrantType> GrantTypes { get; set; }

        public DefaultGrantTypeRepository()
        {
            GrantTypes = new List<GrantType>
            {
                new GrantType(
                    name: "password",
                    identifierName: "username",
                    passcodeName: "password"),
                new GrantType(
                    name: "client",
                    identifierName: "client_id",
                    passcodeName: "client_secret"),
                new GrantType(
                    name: "portal",
                    identifierName: "username",
                    passcodeName: "password",
                    otherRequiredParameters: new List<string> { "portal" })
            };
        }

        public GrantType GetGrantType(string grantTypeName)
        {
            return GrantTypes.SingleOrDefault(gt => gt.Name == grantTypeName);
        }
    }
}
