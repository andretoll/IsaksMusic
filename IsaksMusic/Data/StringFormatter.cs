using IsaksMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Data
{
    public class StringFormatter
    {
        public static string GetCategoryString(ICollection<SongCategory> categories)
        {
            List<string> list = new List<string>();

            foreach (var category in categories)
            {
                list.Add(category.Category.Name);
            }

            return string.Join(", ", list);
        }

        public static string GetDurationFromSeconds(long value)
        {
            var duration = TimeSpan.FromSeconds(value).ToString();

            duration = duration.Remove(0, 3);

            return duration;
        }
    }
}
