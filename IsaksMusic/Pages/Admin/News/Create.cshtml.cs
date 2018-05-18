using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IsaksMusic.Data;
using IsaksMusic.Models;
using System.Net;

namespace IsaksMusic.Pages.Admin.News
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public NewsEntry NewsEntry { get; set; }

        /// <summary>
        /// Open create news page
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            return Page();
        }        

        /// <summary>
        /// Create news entry
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

            NewsEntry.PublishDate = DateTime.Now;
       
            _applicationDbContext.NewsEntries.Add(NewsEntry);
            await _applicationDbContext.SaveChangesAsync();

            Message = "News entry added";

            return RedirectToPage("./Index");
        }              
    }
}