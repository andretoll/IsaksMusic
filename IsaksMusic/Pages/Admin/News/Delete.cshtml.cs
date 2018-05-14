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
    public class DeleteModel : PageModel
    {
        private readonly IsaksMusic.Data.ApplicationDbContext _context;

        public DeleteModel(IsaksMusic.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsEntry = await _context.NewsEntries.FindAsync(id);

            if (NewsEntry != null)
            {
                _context.NewsEntries.Remove(NewsEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
