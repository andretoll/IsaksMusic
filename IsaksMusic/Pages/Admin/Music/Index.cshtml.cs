using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IsaksMusic.Pages.Admin.Music
{
    public class SongsModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SongsModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public SongModel Song { get; set; }

        public class SongModel
        {

        }


        public void OnGet()
        {

        }
    }
}