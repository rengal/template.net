using System.Collections.Generic;
using System.Linq;

namespace Resto.Data
{
    public partial class DateDetalization
    {
        public static List<DateDetalization> AllExceptTotal()
        {
            return VALUES.Where(det => !det.Equals(TOTAL_ONLY)).ToList();
        }
    }
}