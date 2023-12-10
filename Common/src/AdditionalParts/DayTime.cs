using System;

namespace Resto.Data
{
    public partial class DayTime
    {
        public TimeSpan ToTimeSpan()
        {
            return TimeSpan.FromMinutes(Minutes);
        }
    }
}