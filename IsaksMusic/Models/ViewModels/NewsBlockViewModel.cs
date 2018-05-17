using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Models.ViewModels
{
    public class NewsBlockViewModel
    {
        public List<NewsEntryViewModel> NewsEntries { get; set; }
        public bool NoMoreData { get; set; }
    }
}
