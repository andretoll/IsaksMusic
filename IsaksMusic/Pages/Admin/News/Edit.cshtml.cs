using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IsaksMusic.Data;
using IsaksMusic.Models;

namespace IsaksMusic.Pages.Admin.News
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string Message { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            if ((!string.IsNullOrEmpty(NewsEntry.LinkTitle) && string.IsNullOrEmpty(NewsEntry.LinkUrl)) || (string.IsNullOrEmpty(NewsEntry.LinkTitle) && !string.IsNullOrEmpty(NewsEntry.LinkUrl)))
            {
                ModelState.AddModelError("", "Incomplete link setup.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(NewsEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsEntryExists(NewsEntry.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = "Entry updated";

            return RedirectToPage("./Index");
        }

        private bool NewsEntryExists(int id)
        {
            return _context.NewsEntries.Any(e => e.Id == id);
        }
    }
}
