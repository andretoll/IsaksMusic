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
        private readonly ApplicationDbContext _applicationDbContext;

        public EditModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public NewsEntry NewsEntry { get; set; }

        /// <summary>
        /// Get news entry by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            NewsEntry = await _applicationDbContext.NewsEntries.SingleOrDefaultAsync(m => m.Id == id);

            if (NewsEntry == null)
            {
                return NotFound();
            }

            return Page();
        }

        /// <summary>
        /// Edit news entry
        /// </summary>
        /// <returns></returns>
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

            _applicationDbContext.Attach(NewsEntry).State = EntityState.Modified;

            try
            {
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_applicationDbContext.NewsEntries.Any(e => e.Id == NewsEntry.Id))
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
    }
}
