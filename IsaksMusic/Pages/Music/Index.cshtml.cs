using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public IList<Song> Songs { get; set; }

        public async Task OnGet()
        {
            /* List of songs */
            var songs = await _applicationDbContext.Songs.Include(song => song.SongCategories)
                .ThenInclude(songCategories => songCategories.Category).OrderBy(song => song.Title).ToListAsync();

            SongList = new List<SongModel>();

            foreach (var song in songs)
            {
                SongList.Add(new SongModel
                {
                    Title = song.Title,
                    Categories = StringFormatter.GetCategoryString(song.SongCategories),
                    Description = song.Description,
                    Duration = StringFormatter.GetDurationFromSeconds(song.Length),
                    FilePath = "music/" + song.FileName
                });
            }
        }

        public class SongModel
        {
            public string Title { get; set; }
            public string Categories { get; set; }
            public string Duration { get; set; }
            public string Description { get; set; }

            public string FilePath { get; set; }
        }
    }
}