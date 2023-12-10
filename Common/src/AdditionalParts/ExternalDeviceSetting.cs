using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public abstract partial class ExternalDeviceSetting
    {
        [CanBeNull]
        public ExternalDeviceSetting TryMergeWith([CanBeNull] ExternalDeviceSetting oldSetting)
        {
            if (oldSetting != null && oldSetting.Name != Name)
                throw new ArgumentException($"Cannot merge “{Name}” with “{oldSetting.Name}”, settings should have the same name.");
            return TryMergeWithInternal(oldSetting);
        }

        [CanBeNull]
        protected abstract ExternalDeviceSetting TryMergeWithInternal([CanBeNull] ExternalDeviceSetting oldSetting);
    }
}
