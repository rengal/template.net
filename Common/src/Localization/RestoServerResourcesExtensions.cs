using System;
using System.Reflection;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Localization;

namespace Resto.Common.Localization
{
    public static class RestoServerResourcesExtensions
    {
        private static readonly Lazy<Localizer> Localizer = new Lazy<Localizer>(() => CreateLocalizer("RestoServerResources.resx", "Resto.Common.Resources.RestoServerResources"));
        private static readonly Lazy<Localizer> DbSetupLocalizer = new Lazy<Localizer>(() => CreateLocalizer("RestoDbSetupResources.resx", "Resto.Common.Resources.RestoDbSetupResources"));
        private static readonly Lazy<Localizer> OlapLocalizer = new Lazy<Localizer>(() => CreateLocalizer("RestoOlapResources.resx", "Resto.Common.Resources.RestoOlapResources"));

        [Pure]
        private static Localizer CreateLocalizer([NotNull] string fileName, [NotNull] string rmNamespace)
        {
            return Framework.Localization.Localizer.Create(fileName, rmNamespace, Assembly.GetExecutingAssembly());
        }

        [CanBeNull]
        public static string TryGetLocalResource([NotNull] string resourceId)
        {
            if (resourceId == null)
                throw new ArgumentNullException(nameof(resourceId));

            return DbSetupLocalizer.Value.TryGetStringFromResources(resourceId)
                ?? OlapLocalizer.Value.TryGetStringFromResources(resourceId)
                ?? Localizer.Value.TryGetStringFromResources(resourceId);
        }

        [NotNull]
        public static string GetLocalResourceOrDefault([NotNull] string resourceId)
        {
            return TryGetLocalResource(resourceId) ?? resourceId; // тут есть сомнения, какую заглушку использовать — пустую строку или ключ ресурса
        }

        public static string GetLocalName([NotNull] this ILocalizableName target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            return Localizer.Value.GetStringFromResources(target.NameResId);
        }

        [NotNull]
        public static string GetLocalShortName([NotNull] this ILocalizableShortName target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            return Localizer.Value.GetStringFromResources(target.ShortNameResId);
        }

        [NotNull]
        public static string GetLocalDescription([NotNull] this ILocalizableDescription target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            return GetLocalResourceOrDefault(target.DescriptionResId);
        }

        /// <summary>
        /// Возвращает true, если объект используется/отображается для данного edition-a.
        /// Например: "Акты приема топлива" используются только для режима "Петролиум".
        /// </summary>
        public static bool IsUsedInCurrentEdition(this ILocalizableName target)
        {
            return !string.IsNullOrEmpty(target.GetLocalName());
        }
    }
}
