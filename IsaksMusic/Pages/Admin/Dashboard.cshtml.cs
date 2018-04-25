using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IsaksMusic.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DashboardModel(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnGet()
        {
            /* Check for broken links */

            var SongList = _applicationDbContext.Songs.Include(song => song.SongCategories)
                .ThenInclude(songCategories => songCategories.Category).OrderBy(song => song.Title).ToList();

            var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "music");

            for (int i = 0; i < SongList.Count; i++)
            {
                if (!System.IO.File.Exists(fullPath + $@"\{SongList[i].FileName}"))
                {
                    SongList[i].FileName = "Not found";
                }
            }
        }
    }
}