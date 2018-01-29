using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace FCore.FileServer
{
    public class DefaultFileSaver<TUploadedFileInfo> 
        where TUploadedFileInfo : class, IUploadedFileInfo, new()
    {
        private PathsProviderBase PathsProvider { get; set; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        public DefaultFileSaver(PathsProviderBase pathsProvider, IHostingEnvironment hostingEnvironment)
        {
            PathsProvider = pathsProvider;
            HostingEnvironment = hostingEnvironment;
        }

        public TUploadedFileInfo SaveFile(IFormFile formFile, HttpRequest httpRequest, 
            Action<TUploadedFileInfo, IFormFile> saveAdditionalUploadedFileInfo = null,
            Action<TUploadedFileInfo, UploadedFilePaths> saveFileInfoToStore = null)
        {
            string filename = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

            UploadedFilePaths paths = PathsProvider.GenerateUploadedFilePaths(HostingEnvironment, httpRequest, filename);

            using (FileStream output = File.Create(paths.AbsoluteLocalPath))
            {
                formFile.CopyTo(output);
                output.Flush();
            }

            TUploadedFileInfo uploadedFileInfo = new TUploadedFileInfo();
            uploadedFileInfo.Url = paths.AbsoluteUrl;

            if (saveAdditionalUploadedFileInfo != null)
                saveAdditionalUploadedFileInfo.Invoke(uploadedFileInfo, formFile);

            if (saveFileInfoToStore != null)
                saveFileInfoToStore.Invoke(uploadedFileInfo, paths);

            return uploadedFileInfo;
        }
    }
}
