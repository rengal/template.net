using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public sealed partial class ExternalDeviceCustomEnumSetting
    {
        [CanBeNull]
        protected override ExternalDeviceSetting TryMergeWithInternal([CanBeNull] ExternalDeviceSetting oldSettingSrc)
        {
            ExternalDeviceCustomEnumSettingValue newSelected = null;
            if (oldSettingSrc is ExternalDeviceCustomEnumSetting oldEnumSetting)
            {
                var oldSelected = oldEnumSetting.Values.FirstOrDefault(item => item.IsDefault);
                if (oldSelected != null)
                {
                    newSelected = Values.FirstOrDefault(item => item.Name == oldSelected.Name);
                }
            }

            return new ExternalDeviceCustomEnumSetting(Name, Label, IsList)
            {
                Values = Values
                    .Select(item => new ExternalDeviceCustomEnumSettingValue(item.Name, item.Label)
                    {
                        IsDefault = newSelected != null
                            ? newSelected.Name == item.Name
                            : item.IsDefault
                    })
                    .ToList()
            };
        }
    }
}