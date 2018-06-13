using Microsoft.AspNetCore.Http;
using System.IO;

namespace FCore.FileServer
{
    public class DefaultFileSavingMechanism : IFileSavingMechanism
    {
        public void SaveFile(IFormFile formFile, string absoluteLocalPath)
        {
            string pathRoot = Path.GetDirectoryName(absoluteLocalPath);

            if (!Directory.Exists(pathRoot))
                Directory.CreateDirectory(pathRoot);

            using (FileStream output = File.Create(absoluteLocalPath))
            {
                formFile.CopyTo(output);
                output.Flush();
            }            
        }
    }
}
