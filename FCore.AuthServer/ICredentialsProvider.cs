using System.Threading.Tasks;

namespace FCore.AuthServer
{
    public interface ICredentialsProvider
    {
        Task<bool> AreUserCredentialsValidAsync(string username, string password);
        Task<bool> AreClientCredentialsValidAsync(string clientId, string secret);
    }
}
