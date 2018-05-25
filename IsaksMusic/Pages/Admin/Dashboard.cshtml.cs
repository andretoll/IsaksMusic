using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IsaksMusic.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardModel(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        public bool UnusedCategories { get; set; }

        public bool NoFeaturedSong { get; set; }

        public List<string> MissingFiles { get; set; }

        public int NewsRead { get; set; }
        public int SongsPlayed { get; set; }
        public string TrendingSong { get; set; }

        /// <summary>
        /// Get notifications and warnings
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            /* Check for unused categories */
            var categories = await _applicationDbContext.Categories.Where(c => !_applicationDbContext.SongCategories.Select(sc => sc.CategoryId).Contains(c.Id)).ToListAsync();

            if (categories.Count > 0)
            {
                UnusedCategories = true;
            }

            /* Check for featured song */
            var featuredSong = await _applicationDbContext.FeaturedSongs.FirstOrDefaultAsync();

            if (featuredSong == null)
            {
                NoFeaturedSong = true;
            }

            /* Check for broken links */
            var SongList = await _applicationDbContext.Songs.ToListAsync();

            var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "music");

            MissingFiles = new List<string>();

            for (int i = 0; i < SongList.Count; i++)
            {
                if (!System.IO.File.Exists(fullPath + $@"\{SongList[i].FileName}"))
                {
                    MissingFiles.Add(SongList[i].FileName);
                }
            }

            /* Statistics */
            SongsPlayed = await _applicationDbContext.Statistics.CountAsync();
            NewsRead = await _applicationDbContext.NewsEntries.SumAsync(n => n.ReadCount);
            try
            {
                TrendingSong = _applicationDbContext.Statistics.
                    Include(s => s.Song).Where(s => s.PlayedDate >= DateTime.Today.AddDays(-14) && s.PlayedDate <= DateTime.Now).
                    GroupBy(s => s.Song.Title).Select(g => new
                    {
                        Title = g.Key,
                        Count = g.Distinct().Count()
                    }).OrderByDescending(s => s.Count).
                    FirstOrDefault().Title;
            }
            catch
            {
                TrendingSong = "-";
            }
        }

        public class UserModel
        {
            public string Email { get; set; }
            public string Username { get; set; }
        }
    }
}