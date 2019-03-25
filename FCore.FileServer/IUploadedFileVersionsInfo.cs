using System.Collections.Generic;

namespace FCore.FileServer
{
    public interface IUploadedFileVersionsInfo : IUploadedFileInfo
    {
        List<string> VersionUrls { get; set; }
    }
}
