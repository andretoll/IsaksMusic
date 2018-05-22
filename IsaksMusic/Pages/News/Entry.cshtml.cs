using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IsaksMusic.Pages.News
{
    public class EntryModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EntryModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public NewsEntryViewModel NewsEntryViewModel { get; set; }

        /// <summary>
        /// Get news by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task OnGet(int? id)
        {
            var newsEntry = await _applicationDbContext.NewsEntries.Where(n => n.Id == id).SingleOrDefaultAsync();

            NewsEntryViewModel = new NewsEntryViewModel()
            {
                Id = newsEntry.Id,
                Headline = newsEntry.Headline,
                Lead = newsEntry.Lead,
                Body = newsEntry.Body,
                ImageUrl = newsEntry.ImageUrl,
                LinkTitle = newsEntry.LinkTitle,
                LinkUrl = newsEntry.LinkUrl,
                PublishDate = newsEntry.PublishDate.ToLongDateString()
            };

            if (NewsEntryViewModel.Body.Contains("\r\n"))
            {
                string temp = NewsEntryViewModel.Body.Replace("\r\n", "<br>");
                NewsEntryViewModel.Body = temp;
            }

            if (string.IsNullOrEmpty(NewsEntryViewModel.ImageUrl))
            {
                NewsEntryViewModel.ImageUrl = "/images/news-default.jpg";
            }
        }        
    }
}