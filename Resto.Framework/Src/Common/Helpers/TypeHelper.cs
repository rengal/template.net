using System;

namespace Resto.Framework.Common
{
    public static class TypeHelper
    {
        public static bool IsGenericSubclassOf(this Type type, Type generic)
        {
            for (; type != typeof(object); type = type.BaseType)
            {
                if ((type.IsGenericType ? type.GetGenericTypeDefinition() : type) == generic)
                    return true;
            }
            return false;
        }
    }
}
