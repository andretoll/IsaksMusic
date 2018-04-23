using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Models
{
    public class SongCategory
    {
        public int Id { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
