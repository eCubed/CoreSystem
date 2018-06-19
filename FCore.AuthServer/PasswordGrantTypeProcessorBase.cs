using System.Collections.Generic;

namespace FCore.AuthServer
{
    public abstract class PasswordGrantTypeProcessorBase : GrantTypeProcessorBase
    {
        public PasswordGrantTypeProcessorBase() : base("username", "password", new List<string>())
        {
        }
    }
}
