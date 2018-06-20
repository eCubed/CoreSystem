using System.Collections.Generic;

namespace FCore.AuthServer
{
    public abstract class PortalGrantTypeProcessorBase : GrantTypeProcessorBase
    {
        public PortalGrantTypeProcessorBase() : base("username", "password", new List<string> { "portal" })
        {
        }
    }
}
