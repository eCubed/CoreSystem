namespace FCore.AuthServer
{
    public interface IGrantTypeProcessorFactory
    {
        IGrantTypeProcessor CreateInstance(string grantTypeName);

        /* The implementor will need to create the factory, including explicitly including instantiations for the password, client, and portal
         * grant types. The processor factory will be in the DI system, so it needs to know the data context, so the data context may be passed
         * along to the instantiation of a concrete grant type processor base (so that the grant type processor base can perform identifier
         * validation, obtain claims, and formulate the auth server response object).
         */
    }
}
