using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IsaksMusic.Data;
using IsaksMusic.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using IsaksMusic.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace IsaksMusic.Pages.Admin.News
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [TempData]
        public string Message { get; set; }

        public string DateFiltered { get; set; }

        public List<NewsEntryViewModel> NewsEntries { get;set; }

        /// <summary>
        /// Get initial news entries by date
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            /* Get cookie */
            string filterCookie = Request.Cookies["NewsFilter"];

            DateTime filter = new DateTime();

            if (filterCookie != null)
            {
                filter = DateTime.Parse(filterCookie);
            }
            else
            {
                filter = DateTime.Now.AddMonths(-1);
            }

            /* Get news entries the last month */
            var news = await _applicationDbContext.NewsEntries.Where(n => n.PublishDate > filter).OrderByDescending(n => n.PublishDate).ToListAsync();

            NewsEntries = new List<NewsEntryViewModel>();

            foreach (var entry in news)
            {
                NewsEntryViewModel viewModel = new NewsEntryViewModel()
                {
                    Id = entry.Id,
                    Headline = entry.Headline,
                    Lead = entry.Lead,
                    Body = entry.Body,
                    ImageUrl = entry.ImageUrl,
                    LinkTitle = entry.LinkTitle,
                    LinkUrl = entry.LinkUrl,
                    PublishDate = entry.PublishDate.ToLongDateString()
                };

                if (string.IsNullOrEmpty(viewModel.ImageUrl))
                {
                    viewModel.ImageUrl = "/images/news-default.jpg";
                }

                NewsEntries.Add(viewModel);
            }

            DateFiltered = filter.ToShortDateString();
        }

        /// <summary>
        /// Delete a news entry
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDelete(int? id)
        {
            if (id != null)
            {
                /* Get news entry from database */
                var entry = await _applicationDbContext.NewsEntries.Where(n => n.Id == id).SingleOrDefaultAsync();

                /* Remove news entry from database */
                if (entry != null)
                {
                    _applicationDbContext.NewsEntries.Remove(entry);
                    await _applicationDbContext.SaveChangesAsync();
                }                
            }           

            return RedirectToPage();
        }

        /// <summary>
        /// Get news entries by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetFilter(string dateFilter)
        {
            DateTime filter = DateTime.Parse(dateFilter);

            if (filter != null)
            {
                var news = await _applicationDbContext.NewsEntries.Where(n => n.PublishDate > filter).OrderByDescending(n => n.PublishDate).ToListAsync();

                NewsEntries = new List<NewsEntryViewModel>();

                foreach (var entry in news)
                {
                    NewsEntryViewModel viewModel = new NewsEntryViewModel()
                    {
                        Id = entry.Id,
                        Headline = entry.Headline,
                        Lead = entry.Lead,
                        Body = entry.Body,
                        ImageUrl = entry.ImageUrl,
                        LinkTitle = entry.LinkTitle,
                        LinkUrl = entry.LinkUrl,
                        PublishDate = entry.PublishDate.ToLongDateString()
                    };

                    if (string.IsNullOrEmpty(viewModel.ImageUrl))
                    {
                        viewModel.ImageUrl = "/images/news-default.jpg";
                    }

                    NewsEntries.Add(viewModel);
                }

                /* Create partial view with news entries */
                var myViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "_NewsEntries", NewsEntries } };
                myViewData.Model = NewsEntries;

                PartialViewResult result = new PartialViewResult()
                {
                    ViewName = "_NewsEntries",
                    ViewData = myViewData,
                };

                /* Set cookie */
                CookieOptions options = new CookieOptions();

                options.Expires = DateTime.Now.AddMinutes(10);

                Response.Cookies.Append("NewsFilter", filter.ToShortDateString(), options);

                return result;
            }

            return Page();
        }
    }
}
