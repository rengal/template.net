using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.src.Extensions;

public static class AgentDevicesHelper
{
    public static List<Data.ExternalDeviceSetting> MergeSettings([CanBeNull] List<Data.ExternalDeviceSetting> deviceSettings,
        [NotNull] List<Data.ExternalDeviceSetting> defaultSettings)
    {
        if (defaultSettings == null)
            throw new ArgumentNullException(nameof(defaultSettings));

        return defaultSettings
            .Select(defaultSetting =>
            {
                var deviceSetting = deviceSettings?.FirstOrDefault(item => item.Name == defaultSetting.Name);
                return defaultSetting.TryMergeWith(deviceSetting);
            })
            .ToList();
    }
}