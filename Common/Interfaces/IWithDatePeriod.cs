using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    public interface IWithDatePeriod
    {
        DateTime From { get; set; }
        DateTime To { get; set; }
    }
}
