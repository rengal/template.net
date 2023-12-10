using System;

namespace Resto.Data
{
    public partial class DateTimeServerConstants
    {
        /// <summary>
        /// Дата, равная серверной resto.utils.DateUtils.MIN_DATE
        /// (в настоящий момент: 01.01.1900).
        /// </summary>
        public static DateTime MinDate => MIN_DATE.DateTime;

        /// <summary>
        /// Дата, равная серверной resto.utils.DateUtils.MAX_DATE
        /// (в настоящий момент: 01.01.2500).
        /// </summary>
        public static DateTime MaxDate => MAX_DATE.DateTime;
    }
}
