using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoreLibrary.AuthServer
{
    public interface IGrantTypeProcessor
    {
        Task WriteToHttpResponseAsync(HttpResponse response, HttpRequest request, string issuer);
        Task<ManagerResult> ProcessHttpRequestAsync(HttpRequest request, HttpResponse response);
    }
}
