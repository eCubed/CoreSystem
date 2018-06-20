using System.Collections.Generic;

namespace FCore.AuthServer
{
    public abstract class ClientGrantTypeProcessorBase : GrantTypeProcessorBase
    {
        public ClientGrantTypeProcessorBase() : base("client_id", "client_secret", new List<string>())
        {
        }
    }
}
