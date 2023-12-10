using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public sealed partial class ExternalDeviceBooleanSetting
    {
        public ExternalDeviceBooleanSetting(string name, string label, bool value) : this(name, label)
        {
            Value = value;
        }

        [CanBeNull]
        protected override ExternalDeviceSetting TryMergeWithInternal([CanBeNull] ExternalDeviceSetting oldSettingSrc)
        {
            if (oldSettingSrc is ExternalDeviceBooleanSetting oldBoolSetting)
                return new ExternalDeviceBooleanSetting(Name, Label, oldBoolSetting.Value);
            return new ExternalDeviceBooleanSetting(Name, Label, Value);
        }
    }
}
