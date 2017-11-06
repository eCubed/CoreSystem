using System.Threading.Tasks;

namespace CoreLibrary.AuthServer
{
    public interface ICredentialsProvider
    {
        Task<bool> AreCredentialsValidAsync(string uniqueIdentifier, string passCode);
    }
}
