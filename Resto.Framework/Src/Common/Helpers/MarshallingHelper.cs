using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Helpers
{
    public static class MarshallingHelper
    {
        [Pure]
        public static uint OffsetOf<T>([NotNull] Expression<Func<T, object>> field)
        {
            var fieldName = ExpressionHelper.GetPropertyName(field);
            return (uint)Marshal.OffsetOf<T>(fieldName);
        }
    }
}
