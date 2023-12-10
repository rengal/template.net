using System.Collections.Generic;
using Resto.Data;
using Resto.Framework.Common;

namespace Resto.Common
{
    public class AssemblyChartDateComparer : IComparer<AssemblyChart>
    {
        public int Compare(AssemblyChart x, AssemblyChart y)
        {
            if (x == null || y == null)
            {
                throw new RestoException("Values must not be null");
            }

            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            return y.DateFrom.GetValueOrDefault().CompareTo(x.DateFrom.GetValueOrDefault());
        }
    }
}