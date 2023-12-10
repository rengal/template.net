using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public sealed partial class ExternalDeviceNumberSetting
    {
        public ExternalDeviceNumberSetting(string name, string label, decimal value, ExternalDeviceNumberSettingKind settingKind, decimal? minValue, decimal? maxValue)
            : this(name, label, settingKind, minValue, maxValue)
        {
            Value = value;
        }

        [CanBeNull]
        protected override ExternalDeviceSetting TryMergeWithInternal([CanBeNull] ExternalDeviceSetting oldSettingSrc)
        {
            var numberValue = Value;
            if (oldSettingSrc is ExternalDeviceNumberSetting { Value: { } } oldNumberSetting)
            {
                var minValueCorrect = !MinValue.HasValue || oldNumberSetting.Value >= MinValue;
                var maxValueCorrect = !MaxValue.HasValue || oldNumberSetting.Value <= MaxValue;
                if (minValueCorrect && maxValueCorrect)
                    numberValue = oldNumberSetting.Value;
            }

            return new ExternalDeviceNumberSetting(Name, Label, SettingKind, MinValue, MaxValue)
            {
                Value = numberValue,
            };
        }
    }
}
