using System.Collections.Generic;
using System.Linq;

namespace Resto.Framework.Common.Print.Tags
{
    public sealed partial class TagTable
    {
        private sealed class IntArrayComparer : IEqualityComparer<List<int>>
        {
            public bool Equals(List<int> x, List<int> y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(List<int> obj)
            {
                return obj.Count == 0 ? 0 : obj[0].GetHashCode();
            }
        }
    }
}
