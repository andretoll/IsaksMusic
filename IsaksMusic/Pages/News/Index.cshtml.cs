using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IsaksMusic.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IList<NewsEntry> NewsEntries { get; set; }

        public async Task OnGetAsync()
        {
            NewsEntries = await _applicationDbContext.NewsEntries.OrderByDescending(n => n.PublishDate).ToListAsync();
            foreach (var entry in NewsEntries)
            {
                if (string.IsNullOrEmpty(entry.ImageUrl))
                {
                    entry.ImageUrl = "/images/news-default.jpg";
                }
            }
        }
    }
}