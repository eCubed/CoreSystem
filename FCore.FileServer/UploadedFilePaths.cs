namespace FCore.FileServer
{
    public class UploadedFilePaths
    {        
        public string AbsoluteLocalPath { get; set; }
        public string AbsoluteLocalFolder { get; set; }
        public string AbsoluteUrl { get; set; }
        public string AbsoluteUrlDomain { get; set; }
        public string RelativeUrlPath { get; set; }
        public string RelativeLocalPath { get; set; }
        public string FileExtension { get; set; }
    }
}
