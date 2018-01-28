using Microsoft.AspNetCore.Http;
using System;

namespace FCore.FileServer
{
    /// <summary>
    /// Represents the mechanism that actually saves an uploaded file to the system. The implementor
    /// may want a different saving mechanism other than the one we provide (DefaultFileSaver)
    /// </summary>
    public interface IFileSaver<TUploadedFileInfo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile">one of the files from the POST upload request.</param>
        /// <param name="httpRequest">The httpRequest from which we'll obtain the domain of the uploaded file url</param>
        /// <param name="saveAdditionalUploadedFileInfo">Function callback to load more info about the file</param>
        /// <param name="registerFileToStore">Function callback to hand off the uploaded file information to be stored</param>
        /// <returns></returns>
        TUploadedFileInfo SaveFile(IFormFile formFile, HttpRequest httpRequest, 
            Action<TUploadedFileInfo, IFormFile> saveAdditionalUploadedFileInfo = null,
            Action<TUploadedFileInfo, UploadedFilePaths> saveFileInfoToStore = null);
    }
}
