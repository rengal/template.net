using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class ObjectExtensions
    {
        public static bool In<T>(this T objectToCompare, [NotNull] params T[] objects)
        {
            if (objects == null)
                throw new ArgumentNullException(nameof(objects));

            return objects.Contains(objectToCompare);
        }

        public static bool In<T>(this T objectToCompare, T object1)
        {
            return EqualityComparer<T>.Default.Equals(objectToCompare, object1);
        }

        public static bool In<T>(this T objectToCompare, T object1, T object2)
        {
            return EqualityComparer<T>.Default.Equals(objectToCompare, object1)
                || EqualityComparer<T>.Default.Equals(objectToCompare, object2);
        }

        public static bool In<T>(this T objectToCompare, T object1, T object2, T object3)
        {
            return EqualityComparer<T>.Default.Equals(objectToCompare, object1)
                || EqualityComparer<T>.Default.Equals(objectToCompare, object2)
                || EqualityComparer<T>.Default.Equals(objectToCompare, object3);
        }

        public static bool In<T>(this T objectToCompare, T object1, T object2, T object3, T object4)
        {
            return EqualityComparer<T>.Default.Equals(objectToCompare, object1)
                || EqualityComparer<T>.Default.Equals(objectToCompare, object2)
                || EqualityComparer<T>.Default.Equals(objectToCompare, object3)
                || EqualityComparer<T>.Default.Equals(objectToCompare, object4);
        }

        public static bool NotIn<T>(this T objectToCompare, [NotNull] params T[] objects)
        {
            return !objectToCompare.In(objects);
        }

        public static bool NotIn<T>(this T objectToCompare, T object1)
        {
            return !objectToCompare.In(object1);
        }

        public static bool NotIn<T>(this T objectToCompare, T object1, T object2)
        {
            return !objectToCompare.In(object1, object2);
        }

        public static bool NotIn<T>(this T objectToCompare, T object1, T object2, T object3)
        {
            return !objectToCompare.In(object1, object2, object3);
        }

        public static string NullSafeToString(this object obj)
        {
            return obj?.ToString() ?? string.Empty;
        }
    }
}