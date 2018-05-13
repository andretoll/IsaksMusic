using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IsaksMusic.Pages.Admin.News
{
    public class IndexModel : PageModel
    {
        public string TestLink { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            TestLink = "This is a <a href='./songs'>page</a>";
        }
    }
}