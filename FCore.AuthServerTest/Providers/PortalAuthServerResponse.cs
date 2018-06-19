using FCore.AuthServer;
using System.Collections.Generic;

namespace FCore.AuthServerTest.Providers
{
    public class PortalAuthServerResponse : DefaultAuthServerResponse
    {
        public List<string> Portals { get; set; }
    }
}
