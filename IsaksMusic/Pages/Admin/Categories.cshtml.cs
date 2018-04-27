using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IsaksMusic.Data;
using IsaksMusic.Models;
using System.ComponentModel.DataAnnotations;

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

        /* For adding new category */
        [BindProperty]
        public CategoryModel Category { get; set; }

        /* For displaying categories */
        public IList<Category> Categories { get;set; }

        public class CategoryModel
        {
            [MinLength(2, ErrorMessage = "The name must contain more than 2 characters.")]
            [Required(ErrorMessage = "A category needs a name")]
            public string Name { get; set; }
        }

        /// <summary>
        /// Get list of available categories
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            Categories = await _applicationDbContext.Categories.Include(c => c.SongCategories).ToListAsync();
        }

        /// <summary>
        /// Add new category
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            /* If modelstate is valid */
            if (ModelState.IsValid)
            {
                Category category = new Category()
                {
                    Name = Category.Name
                };                

                _applicationDbContext.Categories.Add(category);

                await _applicationDbContext.SaveChangesAsync();

                Message = "Category added";
            }

            return RedirectToPage();
        }
    }
}
