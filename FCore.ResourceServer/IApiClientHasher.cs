namespace FCore.ResourceServer
{
    public interface IApiClientHasher
    {
        bool CheckHash(string apiKey, string clientSecret, string xInputValue, string clientHash);
    }
}
