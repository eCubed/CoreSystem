﻿using Microsoft.AspNetCore.Hosting;
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
            UploadedFilePaths paths = new UploadedFilePaths();
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            string extension = Path.GetExtension(filename);

            string generatedFilename = GenerateServerFilename(filename);
            string relativeFolder = GenerateRelativePath(extension: extension, forUrl: false);
            paths.AbsoluteLocalFolder = Path.Combine(hostingEnvironment.WebRootPath, relativeFolder);
            paths.RelativeLocalPath = Path.Combine(relativeFolder, generatedFilename);
            paths.AbsoluteLocalPath = Path.Combine(paths.AbsoluteLocalFolder, generatedFilename);
            
            paths.FileExtension = extension;
            
            string protocol = httpRequest.IsHttps ? "https" : "http";
            paths.AbsoluteUrlDomain = $"{protocol}://{httpRequest.Host.Value}";
            paths.RelativeUrlPath = $"{GenerateRelativePath(extension: extension, forUrl: true)}/{generatedFilename}";
            paths.AbsoluteUrl = $"{paths.AbsoluteUrlDomain}/{paths.RelativeUrlPath}";
            
            return paths;
        }
    }
}
