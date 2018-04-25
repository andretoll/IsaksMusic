using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IsaksMusic.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IsaksMusic.Controllers
{
    [Produces("application/json")]
    [Route("api/Files")]
    public class FilesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;

        public FilesController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult UploadFilesActionResult(List<IFormFile> files)
        {
            /* Full path to file */
            var fullPath = string.Empty;

            /* File name */
            var fileName = string.Empty;

            if (files != null)
            {
                foreach (var file in files)
                {
                    /* Get allowed file extensions */
                    IConfigurationSection extensionsArraySection = _configuration.GetSection("AudioFileExtensions");
                    var extensionList = extensionsArraySection.GetChildren().ToList().Select(c => c.Value).ToList();

                    /* Get file extension */
                    string fileExtension = Path.GetExtension(file.FileName);

                    /* Check file extension */
                    if (!extensionList.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Invalid filetype.");
                    }

                    if (file.Length > 0 && ModelState.IsValid)
                    {
                        /* Get file name */
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        /* Get music directory */
                        IConfigurationSection directoryArraySection = _configuration.GetSection("MusicDirectory");
                        var musicDirectory = directoryArraySection.GetChildren().ToList().Select(c => c.Value).First();

                        /* Set full path to file */
                        fullPath = Path.Combine(_hostingEnvironment.WebRootPath, musicDirectory) + $@"\{fileName}";

                        /* Upload to directory */
                        using (FileStream fs = System.IO.File.Create(fullPath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}