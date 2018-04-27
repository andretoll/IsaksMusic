using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IsaksMusic.Data;
using IsaksMusic.Models;

namespace IsaksMusic.Pages.Admin.Categories
{
    public class CategoriesModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoriesModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [TempData]
        public string Message { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        /* For displaying categories */
        public IList<Category> Categories { get;set; }

        /// <summary>
        /// Get list of available categories
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            Categories = await _applicationDbContext.Categories.Include(c => c.SongCategories).ToListAsync();
        }
    }
}
