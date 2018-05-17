using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Models.ViewModels
{
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
