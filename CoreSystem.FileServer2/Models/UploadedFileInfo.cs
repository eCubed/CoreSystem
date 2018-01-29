using FCore.FileServer;

namespace CoreSystem.FileServer2.Models
{
    public class UploadedFileInfo : IUploadedFileInfo
    {
        public string Url { get; set; }
        public long Size { get; set; }
    }
}
