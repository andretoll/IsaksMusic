using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IsaksMusic.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IsaksMusic.Pages.Admin.Music
{
    public class SongsModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SongsModel(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public SongModel Song { get; set; }

        public class SongModel
        {
            public string Title { get; set; }

            [Display(Name = "Description (optional)")]
            public string Description { get; set; }
            public byte Seconds { get; set; }
            public byte Minutes { get; set; }
        }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            /* Full path to file */
            var fullPath = string.Empty;

            /* File name */
            var fileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                string PathDB = string.Empty;

                var files = HttpContext.Request.Form.Files;

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        /* Get file name */
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        /* Set full path to file */
                        fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "music") + $@"\{fileName}";

                        /* Upload to directory */
                        using (FileStream fs = System.IO.File.Create(fullPath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }

            return Page();
        }
    }
}