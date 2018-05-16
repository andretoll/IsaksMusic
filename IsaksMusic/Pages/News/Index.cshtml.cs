using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IsaksMusic.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public NewsBlockModel NewsBlock { get; set; }

        public async Task OnGetAsync()
        {
            int blockSize = 3;

            NewsBlock = new NewsBlockModel();

            var news = await _applicationDbContext.NewsEntries.OrderByDescending(n => n.PublishDate).Take(blockSize).ToListAsync();

            NewsBlock.NewsEntries = new List<NewsEntryViewModel>();

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

                NewsBlock.NewsEntries.Add(viewModel);
            }

            if (NewsBlock.NewsEntries.Count < blockSize)
            {
                NewsBlock.NoMoreData = true;
            }
            else
            {
                NewsBlock.NoMoreData = false;
            }
        }

        /// <summary>
        /// Ajax request for getting more news entry
        /// </summary>
        /// <param name="skip"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetNewsBlockAsync(int skip)
        {
            //System.Threading.Thread.Sleep(2000);

            int blockSize = 3;

            NewsBlockModel newsBlock = new NewsBlockModel();

            var news = await _applicationDbContext.NewsEntries.OrderByDescending(n => n.PublishDate).Skip(skip).Take(blockSize).ToListAsync();

            newsBlock.NewsEntries = new List<NewsEntryViewModel>();

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

                newsBlock.NewsEntries.Add(viewModel);
            }

            if (newsBlock.NewsEntries.Count < blockSize)
            {
                newsBlock.NoMoreData = true;
            }
            else
            {
                newsBlock.NoMoreData = false;
            }

            return new JsonResult(newsBlock);
        }

        public class NewsBlockModel
        {
            public List<NewsEntryViewModel> NewsEntries { get; set; }
            public bool NoMoreData { get; set; }
        }

        public class NewsEntryViewModel
        {
            public int Id { get; set; }
            public string Headline { get; set; }
            public string Lead { get; set; }
            public string Body { get; set; }
            public string ImageUrl { get; set; }
            public string LinkTitle { get; set; }
            public string LinkUrl { get; set; }
            public string PublishDate { get; set; }
        }
    }
}