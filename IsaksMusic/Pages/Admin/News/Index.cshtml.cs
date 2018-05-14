﻿using System;
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
        private readonly IsaksMusic.Data.ApplicationDbContext _context;

        public IndexModel(IsaksMusic.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<NewsEntry> NewsEntry { get;set; }

        public async Task OnGetAsync()
        {
            NewsEntry = await _context.NewsEntries.ToListAsync();
        }
    }
}
