using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public long Length { get; set; }

        public int Order { get; set; }

        public ICollection<SongCategory> SongCategories { get; set; }
    }
}
