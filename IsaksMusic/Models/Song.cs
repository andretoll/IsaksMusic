using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string FilePath { get; set; }
        public long Length { get; set; }

        public ICollection<SongCategory> SongCategories { get; set; }
    }
}
