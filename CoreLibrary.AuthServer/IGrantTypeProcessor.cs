using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoreLibrary.AuthServer
{
    public interface IGrantTypeProcessor
    {
        Task<ManagerResult> ProcessCredentialsAsync(HttpRequest request, HttpResponse response);
        
        Task WriteTokenResponseAsync(HttpResponse response, HttpRequest request, string issuer);
    }
}
