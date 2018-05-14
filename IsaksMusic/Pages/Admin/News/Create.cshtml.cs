using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using IsaksMusic.Data;
using IsaksMusic.Models;

namespace IsaksMusic.Pages.Admin.News
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public NewsEntry NewsEntry { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {          
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewsEntry.PublishDate = DateTime.Now;
       
            _applicationDbContext.NewsEntries.Add(NewsEntry);
            await _applicationDbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }        
    }
}