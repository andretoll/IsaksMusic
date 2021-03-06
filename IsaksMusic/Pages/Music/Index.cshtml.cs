﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IsaksMusic.Pages.Music
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public IndexModel(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public List<SongModel> SongList { get; set; }

        public ArrayList Playlist { get; set; }

        public bool Autoplay { get; set; }

        public IList<SelectListItem> CategoryList { get; set; }

        /// <summary>
        /// Get list of songs and categories
        /// </summary>
        /// <param name="autoplay"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(bool autoplay)
        {
            /* List of songs */
            var songs = await _applicationDbContext.Songs.Include(song => song.SongCategories)
                .ThenInclude(songCategories => songCategories.Category).OrderBy(song => song.Title).OrderBy(song => song.Order).ToListAsync();

            /* List of categories */

            SongList = new List<SongModel>();

            int count = 1;
            foreach (var song in songs)
            {
                SongList.Add(new SongModel
                {
                    SongId = song.Id,
                    PlaylistId = count,
                    Title = song.Title,
                    Categories = StringFormatter.GetCategoryString(song.SongCategories),
                    Description = song.Description,
                    Duration = StringFormatter.GetDurationFromSeconds(song.Length),
                    FilePath = "music/" + song.FileName
                });

                count++;
            }

            Playlist = new ArrayList();

            foreach (var song in SongList)
            {
                Playlist.Add(song.FilePath);
            }

            Autoplay = autoplay;

            CategoryList = new List<SelectListItem>();

            /* Get categories from database */
            List<Category> categories = new List<Category>();
            categories = _applicationDbContext.Categories.Where(c => _applicationDbContext.SongCategories.Select(sc => sc.CategoryId).Contains(c.Id)).ToList();

            /* Convert categories into select items */
            foreach (var category in categories)
            {
                CategoryList.Add(new SelectListItem { Text = category.Name });
            }

            return Page();
        }

        public class SongModel
        {
            public int SongId { get; set; }
            public int PlaylistId { get; set; }
            public string Title { get; set; }
            public string Categories { get; set; }
            public string Duration { get; set; }
            public string Description { get; set; }
            public string FilePath { get; set; }
        }
    }
}