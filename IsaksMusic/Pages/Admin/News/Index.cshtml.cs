﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IsaksMusic.Data;
using IsaksMusic.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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

        public IList<NewsEntry> NewsEntries { get;set; }

        public async Task OnGetAsync()
        {
            DateTime month = DateTime.Now.AddMonths(-1);

            NewsEntries = await _applicationDbContext.NewsEntries.Where(n => n.PublishDate > month).OrderByDescending(n => n.PublishDate).ToListAsync();

            foreach (var entry in NewsEntries)
            {
                if (string.IsNullOrEmpty(entry.ImageUrl))
                {
                    entry.ImageUrl = "/images/news-default.jpg";
                }
            }

            DateFiltered = month.ToShortDateString();
        }

        /// <summary>
        /// Delete news entry
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
                NewsEntries = await _applicationDbContext.NewsEntries.Where(n => n.PublishDate > filter).OrderByDescending(n => n.PublishDate).ToListAsync();

                foreach (var entry in NewsEntries)
                {
                    if (string.IsNullOrEmpty(entry.ImageUrl))
                    {
                        entry.ImageUrl = "/images/news-default.jpg";
                    }
                }

                var myViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "_NewsEntries", NewsEntries } };
                myViewData.Model = NewsEntries;

                PartialViewResult result = new PartialViewResult()
                {
                    ViewName = "_NewsEntries",
                    ViewData = myViewData,
                };

                return result;
            }

            return Page();
        }
    }
}
