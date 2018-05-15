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
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [TempData]
        public string Message { get; set; }

        public IList<NewsEntry> NewsEntries { get;set; }

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

        /// <summary>
        /// Delete news entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

            return RedirectToPage();
        }
    }
}
