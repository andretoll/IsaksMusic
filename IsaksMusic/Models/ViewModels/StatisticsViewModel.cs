using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public List<ToplistViewModel> TopNewsRead { get; set; }
        public int TotalNewsRead { get; set; }
    }
}
