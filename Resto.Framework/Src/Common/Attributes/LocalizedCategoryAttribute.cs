using System;
using System.ComponentModel;
using System.Resources;

namespace Resto.Framework.Attributes
{
    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string resourceId, Type type)
            : base(new ResourceManager(type).GetString(resourceId))
        {
        }
    }
}
