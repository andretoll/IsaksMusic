﻿using System;
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

        /* For uploading song */
        [BindProperty]
        public SongModel Song { get; set; }

        /* For displaying songs in table */
        public List<Song> SongList { get; set; }

        /* For displaying categories when uploading song */
        public List<SelectListItem> CategoryList { get; set; }

        [TempData]
        public string Message { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

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

            [Required(ErrorMessage = "Choose a category")]
            [Display(Name = "Music Categories (limit 2)")]
            public List<int> CategoryIds { get; set; }
        }

        public void OnGet()
        {
            /* List of songs */
            SongList = _applicationDbContext.Songs.Include(song => song.SongCategories)
                .ThenInclude(songCategories => songCategories.Category).OrderBy(song => song.Title).ToList();            

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

        public ActionResult OnGetDelete(int? id)
        {
            if (id != null)
            {
                var song = _applicationDbContext.Songs.Where(s => s.Id == id).SingleOrDefault();

                /* Remove entry from database */
                _applicationDbContext.Songs.Remove(song);
                _applicationDbContext.SaveChanges();

                /* Set full path to file */
                var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "music") + $@"\{song.FileName}";

                /* Remove file from server */
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            return RedirectToPage("Songs");
        }

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
                    IConfigurationSection myArraySection = _configuration.GetSection("AudioFileExtensions");
                    var extensionList = myArraySection.GetChildren().ToList().Select(c => c.Value).ToList();

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
    }
}