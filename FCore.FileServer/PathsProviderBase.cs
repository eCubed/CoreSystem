using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FCore.FileServer
{ 
    public abstract class PathsProviderBase
    {
        protected abstract string GenerateServerFilename(string originalFilename);

        /// <summary>
        /// Generates the relative path of the file in the system. The relative path must be equivalent for both
        /// the absolute url and the absolute local path. The forUrl boolean will only generate the forward slash if true,
        /// and the back slash if false.
        /// </summary>
        /// <param name="forUrl"></param>
        /// <returns></returns>
        protected abstract string GenerateRelativePath(string extension, bool forUrl);

        public UploadedFilePaths GenerateUploadedFilePaths(IHostingEnvironment hostingEnvironment, HttpRequest httpRequest, string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            string extension = Path.GetExtension(filename);

            string generatedFilename = GenerateServerFilename(filename);
            string absoluteLocalPath = $"{hostingEnvironment.WebRootPath}\\{GenerateRelativePath(extension: extension, forUrl: false)}";
            
            UploadedFilePaths paths = new UploadedFilePaths();
            paths.AbsoluteLocalPath = $"{absoluteLocalPath}\\{generatedFilename}";
            paths.FileExtension = extension;

            if (!Directory.Exists(absoluteLocalPath))
            {
                Directory.CreateDirectory(absoluteLocalPath);
            }

            string protocol = httpRequest.IsHttps ? "https" : "http";
            paths.AbsoluteUrlDomainOnly = $"{protocol}://{httpRequest.Host.Value}";
            paths.AbsoluteUrl = $"{paths.AbsoluteUrlDomainOnly}//{GenerateRelativePath(extension: extension, forUrl: true)}//{generatedFilename}";

            return paths;
        }
    }
}
