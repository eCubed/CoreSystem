namespace FCore.ResourceServer
{
    public interface IApiClient
    {
        string Name { get; set; }
        string PublicKey { get; set; }
        string PrivateKey { get; set; }

        string GetIdAsString();
    }
}
