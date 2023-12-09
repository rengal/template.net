using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Framework.Common
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetValues<T>()
            where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}