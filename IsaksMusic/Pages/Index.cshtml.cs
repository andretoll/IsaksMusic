using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IsaksMusic.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ContactFormModel Contact { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool PostbackFailed { get; set; }

        public void OnGet()
        {

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
