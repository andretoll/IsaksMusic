using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsaksMusic.Data
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Returns date of first day of current week
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
