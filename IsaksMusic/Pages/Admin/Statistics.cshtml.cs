using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
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

                ViewModel.TotalNewsRead += entry.ReadCount;
            }
        }
    }
}