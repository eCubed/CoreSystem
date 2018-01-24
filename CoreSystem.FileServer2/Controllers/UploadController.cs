using CoreLibrary.FileServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlixML.FileServer2.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private IHostingEnvironment hostingEnvironment { get; set; }
        private PathsProviderBase pathsProvider { get; set; }

        public UploadController(IHostingEnvironment hostingEnvironment, PathsProviderBase pathsProvider)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.pathsProvider = pathsProvider;
        }

        // POST api/values
        //[Authorize(Roles = "user,admin")]
        [HttpPost]
        public IActionResult Post()
        {
            List<string> urls = new List<string>();

            foreach (var file in Request.Form.Files)
            {
                string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                Paths paths = pathsProvider.GeneratePaths(hostingEnvironment, HttpContext.Request, filename);

                using (FileStream output = System.IO.File.Create(paths.AbsoluteLocalPath))
                {
                    file.CopyTo(output);
                    output.Flush();
                }

                urls.Add(paths.AbsoluteUrl);
            }

            return StatusCode(201, urls);
        }


    }
}
