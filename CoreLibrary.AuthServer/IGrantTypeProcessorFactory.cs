namespace CoreLibrary.AuthServer
{
    public interface IGrantTypeProcessorFactory<TAuthServerResponse>
        where TAuthServerResponse : class, IAuthServerResponse, new()
    {
        IGrantTypeProcessor CreateInstance(string grantType, string cryptionKey);
    }
}
