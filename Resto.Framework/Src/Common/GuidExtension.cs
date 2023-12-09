using System;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class GuidExtension                            
    {
        [Pure]
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }
    }
}
