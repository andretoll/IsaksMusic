using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IsaksMusic.Data;
using IsaksMusic.Models;

namespace IsaksMusic.Pages.Admin.News
{
    public class IndexModel : PageModel
    {
        private readonly IsaksMusic.Data.ApplicationDbContext _applicationDbContext;

        public IndexModel(IsaksMusic.Data.ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IList<NewsEntry> NewsEntries { get;set; }

        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            NewsEntries = await _applicationDbContext.NewsEntries.ToListAsync();

            foreach (var entry in NewsEntries)
            {
                if (string.IsNullOrEmpty(entry.ImageUrl))
                {
                    entry.ImageUrl = "/images/news-default.jpg";
                }
            }
        }

        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id != null)
            {
                /* Get news entry from database */
                var entry = await _applicationDbContext.NewsEntries.Where(n => n.Id == id).SingleOrDefaultAsync();

                /* Remove news entry from database */
                if (entry != null)
                {
                    _applicationDbContext.NewsEntries.Remove(entry);
                    await _applicationDbContext.SaveChangesAsync();
                }                
            }
            return Page();
        }
    }
}
