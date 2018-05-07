﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IsaksMusic.Data;
using IsaksMusic.Models;

namespace IsaksMusic.Pages.Music
{
    public class TrackModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TrackModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public SongModel Track { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _applicationDbContext.Songs.Include(s => s.SongCategories)
                .ThenInclude(songCategories => songCategories.Category).OrderBy(s => s.Title).SingleOrDefaultAsync(m => m.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            Track = new SongModel()
            {
                Title = song.Title,
                Description = song.Description,
                Categories = StringFormatter.GetCategoryString(song.SongCategories),
                UploadDate = song.UploadDate.ToShortDateString(),
                FilePath = song.FileName
            };

            return Page();
        }

        public class SongModel
        {
            public string Title { get; set; }
            public string Categories { get; set; }
            public string Description { get; set; }
            public string FilePath { get; set; }
            public string UploadDate { get; set; }
        }
    }
}
