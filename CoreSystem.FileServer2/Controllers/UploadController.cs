using CoreSystem.FileServer2.Models;
using FCore.FileServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlixML.FileServer2.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private IHostingEnvironment HostingEnvironment { get; set; }
        private PathsProviderBase PathsProvider { get; set; }
        private IFileSavingMechanism FileSavingMechanism { get; set; }

        public UploadController(IHostingEnvironment hostingEnvironment, PathsProviderBase pathsProvider, 
            IFileSavingMechanism fileSavingMechanism)
        {
            HostingEnvironment = hostingEnvironment;
            PathsProvider = pathsProvider;
            FileSavingMechanism = fileSavingMechanism;
        }

        // POST api/values
        //[Authorize(Roles = "user,admin")]
        [HttpPost]
        public IActionResult Post()
        {
            List<UploadedFileInfo> uploadedFileInfos = new List<UploadedFileInfo>();

            foreach (var file in Request.Form.Files)
            {  
                UploadedFileInfo uploadedFileInformation = UploadedFileProcessor.ProcessFile<UploadedFileInfo>(
                    formFile: file,
                    httpRequest: HttpContext.Request,
                    pathsProvider: PathsProvider,
                    hostingEnvironment: HostingEnvironment,
                    fileSavingMechanism: FileSavingMechanism,
                    saveAdditionalUploadedFileInfo: (uploadedFileInfo, formFile) =>
                    {
                        uploadedFileInfo.Size = formFile.Length;
                    },
                    saveFileInfoToStore: (uploadedFileInfo, uploadedFilePaths) =>
                    {
                        /* This is the place where we might want to create a database record for the file just
                         * uploaded. The uploadedFileInfo contains the absolute url and any other property about
                         * the file saved in the saveAdditionalUploadedFileInfo callback. The uploadedFilePaths
                         * has properties that break down urls and local paths to relative, absolute, and even
                         * file extensions. There is adequate information in those two parameters that will 
                         * allow us to save uploaded file records in various ways, including the ability to save
                         * a file record and a domain record, and having that file record point to that domain record,
                         * the ability to associate a file record with a file type (by the extension), if we needed.
                         */

                        var dummy = 12;
                    });
                uploadedFileInfos.Add(uploadedFileInformation);
            }

            return StatusCode(201, uploadedFileInfos);
        }


    }
}
