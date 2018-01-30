using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace FCore.FileServer
{
    public static class UploadedFileProcessor 
    {
        //private PathsProviderBase PathsProvider { get; set; }
        //private IHostingEnvironment HostingEnvironment { get; set; }
        
        public static TUploadedFileInfo ProcessFile<TUploadedFileInfo>(IFormFile formFile, HttpRequest httpRequest,
            PathsProviderBase pathsProvider, IHostingEnvironment hostingEnvironment,
            IFileSavingMechanism fileSavingMechanism,
            Action<TUploadedFileInfo, IFormFile> saveAdditionalUploadedFileInfo = null,
            Action<TUploadedFileInfo, UploadedFilePaths> saveFileInfoToStore = null)
            where TUploadedFileInfo : class, IUploadedFileInfo, new()
        {
            string filename = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

            UploadedFilePaths paths = pathsProvider.GenerateUploadedFilePaths(hostingEnvironment, httpRequest, filename);
           
            fileSavingMechanism.SaveFile(formFile, paths.AbsoluteLocalPath);
            
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
