using System;
using System.ComponentModel;
using System.Resources;

namespace Resto.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string resourceId, Type type)
            : base(new ResourceManager(type).GetString(resourceId))
        {
        }
    }
}