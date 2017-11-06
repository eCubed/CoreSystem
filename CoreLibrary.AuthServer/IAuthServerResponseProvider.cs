namespace CoreLibrary.AuthServer
{
    public interface IAuthServerResponseProvider<TAuthServerResponse>
        where TAuthServerResponse : class, IAuthServerResponse
    {
        void FillValues(string uniqueIdentifier, TAuthServerResponse authServerResponse);
    }
}
