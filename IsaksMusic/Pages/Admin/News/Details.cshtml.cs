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
    public class DetailsModel : PageModel
    {
        private readonly IsaksMusic.Data.ApplicationDbContext _context;

        public DetailsModel(IsaksMusic.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public NewsEntry NewsEntry { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsEntry = await _context.NewsEntries.SingleOrDefaultAsync(m => m.Id == id);

            if (NewsEntry == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
