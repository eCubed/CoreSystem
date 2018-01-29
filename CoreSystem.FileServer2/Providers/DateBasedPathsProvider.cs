using FCore.FileServer;
using System;

namespace CoreSystem.FileServer2.Providers
{
    public class DateBasedPathsProvider : PathsProviderBase
    {
        protected override string GenerateRelativePath(string extension, bool forUrl)
        {
            DateTime now = DateTime.Now;
            string slash = (forUrl) ? "/" : "\\";
            return $"{ now.Year}{slash}{ now.Month}";
        }

        protected override string GenerateServerFilename(string originalFilename)
        {
            DateTime now = DateTime.Now;

            return $"{now.ToString("yyyyMMddHHmmssffff")}_{originalFilename}";
        }
    }
}
