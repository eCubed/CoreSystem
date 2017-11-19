using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CoreLibrary.FileServer
{ 
    public abstract class PathsProviderBase
    {
        protected abstract string GenerateServerFilename(string originalFilename);
        protected abstract string GenerateRelativePath(bool forUrl);

        public Paths GeneratePaths(IHostingEnvironment hostingEnvironment, HttpRequest httpRequest, string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            Paths paths = new Paths();
            string generatedFilename = GenerateServerFilename(filename);
            string absoluteLocalPath = $"{hostingEnvironment.WebRootPath}\\{GenerateRelativePath(forUrl: false)}";
            paths.AbsoluteLocalPath = $"{absoluteLocalPath}\\{generatedFilename}";
            
            if (!Directory.Exists(absoluteLocalPath))
            {
                Directory.CreateDirectory(absoluteLocalPath);
            }
            string protocol = httpRequest.IsHttps ? "https" : "http";
            paths.AbsoluteUrl = $"{protocol}://{httpRequest.Host.Value}//{GenerateRelativePath(forUrl: true)}//{generatedFilename}";

            return paths;
        }
    }
}
