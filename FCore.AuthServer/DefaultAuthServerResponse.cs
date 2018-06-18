using System.Collections.Generic;

namespace FCore.AuthServer
{
    public class DefaultAuthServerResponse : AuthServerResponse
    {
        List<string> Roles { get; set; }
    }
}
