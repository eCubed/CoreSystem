using System.Threading.Tasks;

namespace CoreLibrary.AuthServer
{
    public interface ICredentialsProvider
    {
        Task<bool> AreUserCredentialsValidAsync(string username, string password);
        Task<bool> AreClientCredentialsValidAsync(string clientId, string secret);
    }
}
