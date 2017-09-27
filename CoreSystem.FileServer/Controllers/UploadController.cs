using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreSystem.FileServer.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private IHostingEnvironment hostingEnvironment { get; set; }

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        // POST api/values
        [Authorize(Roles = "user,admin")]
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            foreach(var file in Request.Form.Files)
            {
                string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                filename = EnsureCorrectFilename(filename);

                using (FileStream output = System.IO.File.Create(GetPathAndFilename(filename)))
                {
                    file.CopyTo(output);
                    output.Flush();
                }
            }

            return await Task.FromResult(StatusCode(201));
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            DateTime now = DateTime.Now;

            string dirPath = $"{hostingEnvironment.WebRootPath}\\{now.Year}\\{now.Month}";
            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }

            return $"{dirPath}\\{now.ToString("yyyyMMddHHmmssffff")}_{filename}";
        }

    }
}
