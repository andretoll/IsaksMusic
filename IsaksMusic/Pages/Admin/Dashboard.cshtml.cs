using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IsaksMusic.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardModel(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        public UserModel SignedInUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            SignedInUser = new UserModel
            {
                Username = user.UserName,
                Email = user.Email
            };

            /* Check for broken links */
            //var SongList = _applicationDbContext.Songs.Include(song => song.SongCategories)
            //    .ThenInclude(songCategories => songCategories.Category).OrderBy(song => song.Title).ToList();

            //var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "music");

            //for (int i = 0; i < SongList.Count; i++)
            //{
            //    if (!System.IO.File.Exists(fullPath + $@"\{SongList[i].FileName}"))
            //    {
            //        SongList[i].FileName = "Not found";
            //    }
            //}

            return Page();
        }

        public class UserModel
        {
            public string Email { get; set; }
            public string Username { get; set; }
        }
    }
}