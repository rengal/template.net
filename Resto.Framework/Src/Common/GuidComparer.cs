using System;
using System.Collections.Generic;

namespace Resto.Framework.Common
{
    public sealed class GuidComparer : IEqualityComparer<Guid>
    {
        public static readonly IEqualityComparer<Guid> Default = new GuidComparer();

        public bool Equals(Guid x, Guid y)
        {
            unsafe
            {
                var px = (int*)&x;
                var py = (int*)&y;
                return (px[3] == py[3]) && (px[2] == py[2]) && (px[1] == py[1]) && (px[0] == py[0]);
            }
        }

        public int GetHashCode(Guid obj)
        {
            unsafe
            {
                var p = (int*)&obj;
                return p[0] ^ p[1] ^ p[2] ^ p[3];
            }
        }
    }
}
