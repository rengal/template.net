using System;
using System.ComponentModel;
using System.Globalization;
using Resto.Framework.Properties;

namespace Resto.Framework.Common.Crypto.PropertyGrid
{
    public sealed class ChildObjectConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
          object value, Type destType)
        {
            if (value == null)
            {
                return Resources.ChildObjectConverter_ConvertTo;
            }
            else
            {
                return String.Empty;
            }
        }

    }
}