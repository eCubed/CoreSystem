using System;
using System.IO;

namespace FCore.FileServer
{
    public class DateBasedPathsProvider : PathsProviderBase
    {
        protected override string GenerateRelativePath(string extension, bool forUrl)
        {
            DateTime now = DateTime.Now;
            string slash = (forUrl) ? @"/" : @"\";
            return $"{now.Year}{slash}{now.Month}";
        }

        protected override string GenerateServerFilename(string originalFilename)
        {
            DateTime now = DateTime.Now;
            string extension = Path.GetExtension(originalFilename);
            return $"{now.ToString("yyyyMMddHHmmssffff")}{extension}";
        }
    }
}
