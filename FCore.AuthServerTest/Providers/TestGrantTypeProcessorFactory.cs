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

            return null;
        }
    }
}
