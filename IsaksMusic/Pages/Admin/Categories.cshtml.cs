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

        [BindProperty]
        public CategoryModel Category { get; set; }

        public IList<Category> Categories { get;set; }        

        /// <summary>
        /// Get categories ordered by song count
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            /* Get categories and order by number of songs */
            Categories = await _applicationDbContext.Categories.Include(c => c.SongCategories).ToListAsync();
            Categories = Categories.OrderByDescending(c => c.SongCategories.Count()).ToList();
        }

        /// <summary>
        /// Add new category
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
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

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id != null)
            {
                var category = _applicationDbContext.Categories.Where(c => c.Id == id).Include(c => c.SongCategories).SingleOrDefault();

                /* Check if category has any songs */
                if (category.SongCategories.Count() > 0)
                {
                    ErrorMessage = "Category could not be removed because one or several songs depend on it.";
                    return StatusCode(400);
                }

                _applicationDbContext.Categories.Remove(category);
                await _applicationDbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        /// <summary>
        /// Rename a category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetEdit(int? id, string name)
        {
            if (id != null)
            {
                var category = _applicationDbContext.Categories.Where(c => c.Id == id).SingleOrDefault();

                /* Change category name */
                category.Name = name;
                await _applicationDbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public class CategoryModel
        {
            [MinLength(2, ErrorMessage = "The name must contain more than 2 characters.")]
            [Required(ErrorMessage = "A category needs a name")]
            public string Name { get; set; }
        }
    }
}
