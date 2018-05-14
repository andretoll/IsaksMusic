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
        }
    }
}
