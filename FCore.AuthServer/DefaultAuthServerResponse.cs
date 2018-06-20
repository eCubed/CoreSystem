using System.Collections.Generic;

namespace FCore.AuthServer
{
    public class DefaultAuthServerResponse : BasicAuthServerResponse
    {
        public List<string> Roles { get; set; }
    }
}
