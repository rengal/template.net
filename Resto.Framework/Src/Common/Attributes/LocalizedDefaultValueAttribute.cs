using System;
using System.ComponentModel;
using System.Resources;

namespace Resto.Framework.UI.Common
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class LocalizedDefaultValueAttribute : DefaultValueAttribute
    {
        public LocalizedDefaultValueAttribute(string resourceId, Type type)
            : base(new ResourceManager(type).GetString(resourceId))
        {
        }
    }
}
