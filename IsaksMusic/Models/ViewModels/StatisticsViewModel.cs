using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public string CurrentMonth
        {
            get
            {
                return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(DateTime.Today.Month);
            }
        }

        public List<ToplistViewModel> TopNewsRead { get; set; }
        public List<ToplistViewModel> TopSongsPlayed { get; set; }

        public int SongsPlayedYear { get; set; }
        public int SongsPlayedMonth { get; set; }
        public int SongsPlayedWeek { get; set; }
        public int SongsPlayedToday { get; set; }

        public string MostPlayedYear { get; set; }
        public string MostPlayedMonth { get; set; }
        public string MostPlayedWeek { get; set; }
        public string MostPlayedToday { get; set; }
    }
}
