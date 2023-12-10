using System;
using Resto.Data;

namespace Resto.Common.src.FormatProviders
{
    /// <summary>
    /// FormatProvider для типа Product.
    /// </summary>
    public class ProductFormatProvider: IFormatProvider, ICustomFormatter
    {
        public const string ArticulFormatString = "A";
        public object GetFormat(Type type)
        {
            return this;
        }

        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (!(arg is Product)) return arg.ToString();

            var product = (Product) arg;

            switch (format)
            {
                case ArticulFormatString:
                    return product.Num;
                default:
                    return product.ToString();
            }
        }

        public static ProductFormatProvider Instance = new ProductFormatProvider();
    }
}
