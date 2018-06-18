using System.Collections.Generic;

namespace FCore.AuthServer
{
    public class DefaultAuthServerResponse : AuthServerResponse
    {
        public List<string> Roles { get; set; }
    }
}
