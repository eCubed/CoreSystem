using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace FCore.FileServer
{
    public static class UploadedFileProcessor 
    {   
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

        public static TUploadedFileInfo ProcessFileForVersions<TUploadedFileInfo>(IFormFile formFile, HttpRequest httpRequest,
            PathsProviderBase pathsProvider, IHostingEnvironment hostingEnvironment,
            IFileSavingMechanism fileSavingMechanism,
            Func<string, List<string>> determineFileVersions,
            Action<string, string> saveFileVersion,
            Action<TUploadedFileInfo, IFormFile> saveAdditionalUploadedFileInfo = null,
            Action<TUploadedFileInfo, UploadedFilePaths> saveFileInfoToStore = null)
            where TUploadedFileInfo : class, IUploadedFileVersionsInfo, new()
        {
            #region Processing the principal file, which was just uploaded
            string filename = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

            UploadedFilePaths paths = pathsProvider.GenerateUploadedFilePaths(hostingEnvironment, httpRequest, filename);

            fileSavingMechanism.SaveFile(formFile, paths.AbsoluteLocalPath);

            TUploadedFileInfo uploadedFileInfo = new TUploadedFileInfo();
            uploadedFileInfo.Url = paths.AbsoluteUrl;

            if (saveAdditionalUploadedFileInfo != null)
                saveAdditionalUploadedFileInfo.Invoke(uploadedFileInfo, formFile);

            if (saveFileInfoToStore != null)
                saveFileInfoToStore.Invoke(uploadedFileInfo, paths);

            #endregion

            /* Now that we've saved the file that was just uploaded, we will need to process additional versions, most likely
             * lower resolution and/or thumbnails.
             */
            List<string> filenameVersions = determineFileVersions.Invoke(filename);
            uploadedFileInfo.VersionUrls = filenameVersions;

            filenameVersions.ForEach(filenameVersion =>
            {
                UploadedFilePaths pathsForFileVersion = pathsProvider.GenerateUploadedFilePaths(hostingEnvironment, httpRequest, filenameVersion);

                saveFileVersion.Invoke(paths.AbsoluteLocalPath, pathsForFileVersion.AbsoluteLocalPath);
            });

            return uploadedFileInfo;

        }
    }
}
