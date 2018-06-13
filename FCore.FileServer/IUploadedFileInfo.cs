using System;
using System.Collections.Generic;
using System.Text;

namespace FCore.FileServer
{
    /// <summary>
    /// Represents information about the file saved as returned back to the uploader.
    /// At a minimum, an IUploadedFileInfo must have the absolute url of the file uploaded.
    /// Implementors can add MediaId if they require saving a database record for it, file size,
    /// and other info they find appropriate.
    /// </summary>
    public interface IUploadedFileInfo
    {
        string Url { get; set; }
    }
}
