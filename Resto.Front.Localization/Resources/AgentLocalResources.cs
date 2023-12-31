﻿using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.Localization.Resources
{
    public partial class AgentLocalResources
    {
        [CanBeNull]
        public static string TryGetResource([NotNull] string resourceCode)
        {
            if (resourceCode == null)
                throw new ArgumentNullException(nameof(resourceCode));

            return Localizer.TryGetStringFromResources(resourceCode);
        }
    }
}
