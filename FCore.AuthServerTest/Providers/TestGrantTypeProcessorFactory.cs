using FCore.AuthServer;

namespace FCore.AuthServerTest.Providers
{
    public class TestGrantTypeProcessorFactory : IGrantTypeProcessorFactory
    {
        public IGrantTypeProcessor CreateInstance(string grantTypeName)
        {
            if (grantTypeName == "password")
            {
                return new PasswordGrantTypeProcessor();
            }
            else if (grantTypeName == "portal")
            {
                return new PortalGrantTypeProcessor();
            }

            return null;
        }
    }
}
