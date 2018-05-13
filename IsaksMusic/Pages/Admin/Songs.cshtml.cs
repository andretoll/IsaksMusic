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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace IsaksMusic.Pages.Admin.Music
{
    public class SongsModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public SongsModel(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        [TempData]
        public string Message { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        /* For uploading song */
        [BindProperty]
        public SongUploadModel Song { get; set; }

        /* For displaying songs in table */
        public List<SongModel> SongList { get; set; }

        /* For displaying categories when uploading song */
        public IList<SelectListItem> CategoryList { get; set; }             

        /// <summary>
        /// Get list of songs and categories
        /// </summary>
        public async Task OnGet()
        {
            /* List of songs */
            var songs = await _applicationDbContext.Songs.Include(song => song.SongCategories)
                .ThenInclude(songCategories => songCategories.Category).OrderBy(song => song.Title).OrderBy(song => song.Order).ToListAsync();

            SongList = new List<SongModel>();

            /* Get featured song, if any */
            var featuredSong = await _applicationDbContext.FeaturedSongs.FirstOrDefaultAsync();

            foreach (var song in songs)
            {
                SongModel newSong = new SongModel()
                {
                    Id = song.Id,
                    Title = song.Title,
                    Description = song.Description,
                    Duration = StringFormatter.GetDurationFromSeconds(song.Length),
                    Categories = StringFormatter.GetCategoryString(song.SongCategories),
                    UploadDate = song.UploadDate.ToShortDateString(),
                    FileName = song.FileName
                };

                if (featuredSong != null && featuredSong.SongId == song.Id)
                {
                    newSong.Featured = true;
                }

                SongList.Add(newSong);
            }

            /* Select List with categories */
            CategoryList = new List<SelectListItem>();

            /* Get categories from database */
            List<Category> categories = new List<Category>();
            categories = _applicationDbContext.Categories.ToList();

            /* Convert categories into select items */
            foreach (var category in categories)
            {
                CategoryList.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Name });
            }
        }

        /// <summary>
        /// Upload file to server and save song in database
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            /* Full path to file */
            var fullPath = string.Empty;

            /* File name */
            var fileName = string.Empty;

            if (HttpContext.Request.Form.Files != null)
            {
                var files = HttpContext.Request.Form.Files;

                foreach (var file in files)
                {
                    /* Get allowed file extensions */
                    ApplicationConfigurationRetriever retriever = new ApplicationConfigurationRetriever(_configuration);
                    var extensionList = retriever.GetMusicFileExtensions();

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

                        /* Set full path to file */
                        fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "music") + $@"\{fileName}";

                        if (System.IO.File.Exists(fullPath))
                        {
                            ModelState.AddModelError("", "The file already exists.");
                        }

                        if (ModelState.IsValid)
                        {
                            /* Upload to directory */
                            using (FileStream fs = System.IO.File.Create(fullPath))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
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

                if (_applicationDbContext.Songs.Count() == 0)
                {
                    song.Order = 1;
                }
                else
                {
                    song.Order = _applicationDbContext.Songs.Max(s => s.Order) + 1;
                }

                _applicationDbContext.Songs.Add(song);

                await _applicationDbContext.SaveChangesAsync();

                /* Get song ID */
                int songId = song.Id;

                /* Add song categories to database */
                foreach (var category in Song.CategoryIds)
                {
                    SongCategory songCategory = new SongCategory()
                    {
                        SongId = songId,
                        CategoryId = category
                    };

                    _applicationDbContext.SongCategories.Add(songCategory);
                }

                await _applicationDbContext.SaveChangesAsync();

                Message = "Song added";

                return RedirectToPage();
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                ErrorMessage = errors.FirstOrDefault().FirstOrDefault().ErrorMessage;
            }

            return RedirectToPage();
        }

        /// <summary>
        /// Delete a song
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id != null)
            {
                /* Get song from database */
                var song = _applicationDbContext.Songs.Where(s => s.Id == id).SingleOrDefault();

                /* Remove entry from database */
                _applicationDbContext.Songs.Remove(song);
                await _applicationDbContext.SaveChangesAsync();

                /* Get full path to file */
                var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "music") + $@"\{song.FileName}";

                /* Remove file from server */
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                Message = "Song removed";
            }

            return RedirectToPage();
        }

        /// <summary>
        /// Set feature status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="featured"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetFeature(int? id, bool featured)
        {
            if (id != null)
            {
                var featuredSong = await _applicationDbContext.FeaturedSongs.FirstOrDefaultAsync();
                var song = await _applicationDbContext.Songs.Where(s => s.Id == id).SingleOrDefaultAsync();

                if (!featured)
                {
                    _applicationDbContext.FeaturedSongs.Remove(featuredSong);
                }
                else
                {
                    if (featuredSong == null)
                    {
                        _applicationDbContext.FeaturedSongs.Add(new FeaturedSong
                        {
                            Song = song
                        });
                    }
                    else
                    {
                        featuredSong.Song = song;
                    }
                }                            

                await _applicationDbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        /// <summary>
        /// Reorder songs
        /// </summary>
        /// <param name="orderString"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetReorder(string orderString)
        {
            /* Get song ID array from string */
            string[] orderArray = orderString.Split(',');

            var songs = await _applicationDbContext.Songs.ToListAsync();

            int order = 1;

            /* For each song */
            foreach (var song in orderArray)
            {
                /* Get song */
                var songDb = songs.Where(s => s.Id == int.Parse(song)).FirstOrDefault();

                songDb.Order = order;

                order++;
            }

            await _applicationDbContext.SaveChangesAsync();

            return Page();
        }

        public class SongModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Duration { get; set; }
            public string Categories { get; set; }
            public string UploadDate { get; set; }
            public string FileName { get; set; }
            public bool Featured { get; set; }
        }

        public class SongUploadModel
        {
            [Required(ErrorMessage = "A song needs a title")]
            public string Title { get; set; }

            [Display(Name = "Description (optional)")]
            public string Description { get; set; }

            [Range(0, 59, ErrorMessage = "Not a valid number for minutes")]
            [Required(ErrorMessage = "Please specify minutes")]
            public byte Minutes { get; set; }

            [Range(0, 59, ErrorMessage = "Not a valid number for seconds")]
            [Required(ErrorMessage = "Please specify seconds")]
            public byte Seconds { get; set; }

            [Display(Name = "Music file (.mp3 .wav)")]
            [Required(ErrorMessage = "Choose a file to upload")]
            public IFormFile MusicFile { get; set; }

            [Required(ErrorMessage = "Choose a category")]
            [Display(Name = "Music Categories (limit 2)")]
            public List<int> CategoryIds { get; set; }
        }
    }
}