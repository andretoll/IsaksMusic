using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public List<Song> SongList { get; set; }

        public class SongModel
        {
            [Required(ErrorMessage = "A song needs a title")]
            public string Title { get; set; }

            [MaxLength(length: 150, ErrorMessage = "Description cannot exceed 150 characters.")]
            [Display(Name = "Description (optional)")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Please specify minutes")]
            public byte Minutes { get; set; }

            [Required(ErrorMessage = "Please specify seconds")]
            public byte Seconds { get; set; }

            [Required(ErrorMessage = "Choose a file to upload")]
            public IFormFile MusicFile { get; set; }
        }

        public void OnGet()
        {
            SongList = new List<Song>();

            SongList = _applicationDbContext.Songs.ToList();
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

            /* If modelstate is valid */
            if (ModelState.IsValid)
            {
                TimeSpan songLength = new TimeSpan(0, Song.Minutes, Song.Seconds);
                long seconds = long.Parse(songLength.TotalSeconds.ToString());

                /* Create entry to database */
                Song song = new Song()
                {
                    Title = Song.Title,
                    Description = Song.Description,
                    Length = seconds,
                    UploadDate = DateTime.Now,
                    FileName = fileName
                };

                _applicationDbContext.Songs.Add(song);
                await _applicationDbContext.SaveChangesAsync();

                return RedirectToPage();
            }

            return RedirectToPage();

        }
    }
}