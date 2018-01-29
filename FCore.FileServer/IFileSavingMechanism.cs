using Microsoft.AspNetCore.Http;

namespace FCore.FileServer
{
    /// <summary>
    /// Represents the mechanism that actually saves an uploaded file to the system. The implementor
    /// may want a different saving mechanism other than the one we provide (DefaultFileSavingMechanism)
    /// </summary>
    public interface IFileSavingMechanism
    {        
        void SaveFile(IFormFile formFile, string absoluteLocalPath);
    }
}
