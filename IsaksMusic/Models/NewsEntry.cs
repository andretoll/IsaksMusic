using IsaksMusic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Models
{
    public class NewsEntry
    {
        public int Id { get; set; }

        [Required]
        public string Headline { get; set; }


        [Required]
        public string Lead { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime PublishDate { get; set; }

        [Display(Name = "Image URL (optional)")]
        [Url(ErrorMessage = "Not valid URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Link text (optional)")]
        public string LinkTitle { get; set; }

        [Display(Name = "Link URL (optional)")]
        [Url(ErrorMessage = "Not valid URL")]
        public string LinkUrl { get; set; }
    }
}
