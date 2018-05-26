using System;
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

        /// <summary>
        /// Get track by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task OnGetAsync(int? id)
        {
            if (id != null)
            {
                var song = await _applicationDbContext.Songs.Include(s => s.SongCategories)
                .ThenInclude(songCategories => songCategories.Category).OrderBy(s => s.Title).SingleOrDefaultAsync(m => m.Id == id);

                Track = new SongModel()
                {
                    Id = song.Id,
                    Title = song.Title,
                    Description = song.Description,
                    Categories = StringFormatter.GetCategoryString(song.SongCategories),
                    UploadDate = song.UploadDate.ToShortDateString(),
                    FilePath = "/music/" + song.FileName
                };

                int songId = (int)id;
                Statistics statistics = new Statistics()
                {
                    SongId = songId,
                    PlayedDate = DateTime.Now.Date
                };

                await _applicationDbContext.Statistics.AddAsync(statistics);
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get random track
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetRandomAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songs = await _applicationDbContext.Songs.Include(s => s.SongCategories).ThenInclude(sc => sc.Category).ToListAsync();

            songs.RemoveAll(s => s.Id == id);

            /* Get random track, but not the same */
            Random rand = new Random();
            int toSkip = rand.Next(0, songs.Count());
            var song = songs.Skip(toSkip).FirstOrDefault();

            Track = new SongModel()
            {
                Id = song.Id,
                Title = song.Title,
                Description = song.Description,
                Categories = StringFormatter.GetCategoryString(song.SongCategories),
                UploadDate = song.UploadDate.ToShortDateString(),
                FilePath = "/music/" + song.FileName
            };

            int songId = song.Id;
            Statistics statistics = new Statistics()
            {
                SongId = songId,
                PlayedDate = DateTime.Now.Date
            };

            await _applicationDbContext.Statistics.AddAsync(statistics);
            await _applicationDbContext.SaveChangesAsync();

            return Page();
        }

        public class SongModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Categories { get; set; }
            public string Description { get; set; }
            public string FilePath { get; set; }
            public string UploadDate { get; set; }
        }
    }
}
