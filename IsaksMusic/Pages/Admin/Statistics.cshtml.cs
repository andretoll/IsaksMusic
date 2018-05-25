using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Models;
using IsaksMusic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IsaksMusic.Pages.Admin
{
    public class StatisticsModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StatisticsModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public StatisticsViewModel ViewModel { get; set; }

        public async Task OnGet()
        {
            int blockSize = 5;

            /* Get top news */
            var topNews = await _applicationDbContext.NewsEntries.OrderByDescending(n => n.ReadCount).Take(blockSize).ToListAsync();

            ViewModel = new StatisticsViewModel();
            ViewModel.TopNewsRead = new List<ToplistViewModel>();

            foreach (var entry in topNews)
            {
                ViewModel.TopNewsRead.Add(new ToplistViewModel
                {
                    Title = entry.Headline,
                    Points = entry.ReadCount
                });
            }

            /* Get top songs */
            var topSongs = _applicationDbContext.Statistics.Include(s => s.Song).GroupBy(s => s.Song.Title).Select(g => new {
                Title = g.Key,
                Count = g.Distinct().Count()
            }).OrderByDescending(s => s.Count).Take(blockSize).ToList();

            ViewModel.TopSongsPlayed = new List<ToplistViewModel>();

            foreach (var song in topSongs)
            {
                ViewModel.TopSongsPlayed.Add(new ToplistViewModel
                {
                    Title = song.Title,
                    Points = song.Count
                });
            }

            /* Get total songs played by time */
            var allStatistics = await _applicationDbContext.Statistics.Where(s => s.PlayedDate.Year == DateTime.Today.Year).ToListAsync();
            ViewModel.SongsPlayedYear = allStatistics.Count;
            ViewModel.SongsPlayedMonth = allStatistics.Where(s => s.PlayedDate.Month == DateTime.Today.Month).Count();

            DateTime startOfWeek = DateTimeHelper.StartOfWeek(DateTime.Today, DayOfWeek.Monday);
            ViewModel.SongsPlayedWeek = allStatistics.Where(s => s.PlayedDate.Date >= startOfWeek).Count();
            ViewModel.SongsPlayedToday = allStatistics.Where(s => s.PlayedDate.Date == DateTime.Today.Date).Count();

            /* Get most played song by time */
            try
            {
                var topSong = _applicationDbContext.Statistics.Include(s => s.Song).Where(s => s.PlayedDate.Year == DateTime.Today.Year).GroupBy(s => s.Song.Title).Select(g => new {
                    Title = g.Key,
                    Count = g.Distinct().Count()
                }).OrderByDescending(s => s.Count).FirstOrDefault();
                ViewModel.MostPlayedYear = topSong.Title;

                topSong = _applicationDbContext.Statistics.Include(s => s.Song).Where(s => s.PlayedDate.Month == DateTime.Today.Month).GroupBy(s => s.Song.Title).Select(g => new {
                    Title = g.Key,
                    Count = g.Distinct().Count()
                }).OrderByDescending(s => s.Count).FirstOrDefault();
                ViewModel.MostPlayedMonth = topSong.Title;

                topSong = _applicationDbContext.Statistics.Include(s => s.Song).Where(s => s.PlayedDate.Date >= startOfWeek).GroupBy(s => s.Song.Title).Select(g => new {
                    Title = g.Key,
                    Count = g.Distinct().Count()
                }).OrderByDescending(s => s.Count).FirstOrDefault();
                ViewModel.MostPlayedWeek = topSong.Title;

                topSong = _applicationDbContext.Statistics.Include(s => s.Song).Where(s => s.PlayedDate.Date == DateTime.Today.Date).GroupBy(s => s.Song.Title).Select(g => new {
                    Title = g.Key,
                    Count = g.Distinct().Count()
                }).OrderByDescending(s => s.Count).FirstOrDefault();
                ViewModel.MostPlayedToday = topSong.Title;
            }
            catch
            {
            }            
        }

        public async Task OnGetSongCount(int? id)
        {
            if (id != null)
            {
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
    }
}