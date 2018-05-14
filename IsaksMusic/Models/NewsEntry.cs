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

        public string Lead { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime PublishDate { get; set; }
        public string ImageUrl { get; set; }
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
    }
}
