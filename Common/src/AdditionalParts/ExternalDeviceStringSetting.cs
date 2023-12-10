using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public sealed partial class ExternalDeviceStringSetting
    {
        public ExternalDeviceStringSetting(string name, string label, string value, int maxLength)
            : this(name, label, value)
        {
            MaxLength = maxLength;
        }

        [CanBeNull]
        protected override ExternalDeviceSetting TryMergeWithInternal([CanBeNull] ExternalDeviceSetting oldSettingSrc)
        {
            var stringValue = Value;
            if (oldSettingSrc is ExternalDeviceStringSetting oldStringSetting &&
                !string.IsNullOrEmpty(oldStringSetting.Value))
            {
                if (MaxLength <= 0 || oldStringSetting.Value.Length <= MaxLength)
                    stringValue = oldStringSetting.Value;
            }

            return new ExternalDeviceStringSetting(Name, Label, stringValue, MaxLength);
        }
    }
}
