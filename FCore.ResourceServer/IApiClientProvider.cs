namespace FCore.ResourceServer
{
    public interface IApiClientProvider<TApiClient>
        where TApiClient : class, IApiClient
    {
        bool ClientExists(string clientId);
        string GetClientSecret(string clientId);
        TApiClient GetClientByPublicKey(string publicKey);
    }
}
