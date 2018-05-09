﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IsaksMusic.Pages
{
    public class IndexModel : PageModel
    {
        ApplicationDbContext _applicationDbContext;

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public ContactFormModel Contact { get; set; }

        public FeaturedModel Featured { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool PostbackFailed { get; set; }

        public async Task OnGet()
        {
            /* Get featured song, if any */
            var featuredSong = await _applicationDbContext.FeaturedSongs.Include(f => f.Song).FirstOrDefaultAsync();

            if (featuredSong != null)
            {
                Featured = new FeaturedModel
                {
                    SongId = featuredSong.SongId,
                    Title = featuredSong.Song.Title
                };
            }
        }

        /* On contact submit */
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PostbackFailed = true;
                return Page();
            }

            /* Send email here */

            /* Show confirmation */
            Message = "Email successfully sent!";

            return RedirectToPage("Index");
        }

        public class FeaturedModel
        {
            public int SongId { get; set; }
            public string Title { get; set; }
        }

        public class ContactFormModel
        {
            [Required(ErrorMessage = "Please enter your first name")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Please enter your last name")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Please enter your email address")]
            [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Please enter a message")]
            public string Message { get; set; }
        }
    }
}
