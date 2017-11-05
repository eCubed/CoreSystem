namespace CoreLibrary.AuthServer
{
    public interface IGrantTypeProcessorFactory
    {
        IGrantTypeProcessor CreateInstance(string grantType, string cryptionKey);
    }
}
