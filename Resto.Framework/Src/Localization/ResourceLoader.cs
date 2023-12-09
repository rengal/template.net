using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

using Resto.Framework.Attributes.JetBrains;

using Resto.Framework.Common;

namespace Resto.Framework.Localization
{
    /// <summary>
    /// Загрузчик файлов, содержащих языковые локализации (уточнения) ресурсов.
    /// </summary>
    public static class ResourceLoader
    {
        #region Private Members

        private static readonly LogWrapper LogWrapper = new LogWrapper(typeof(ResourceLoader));
        private static readonly Regex BracesCounterEx = new Regex(@"\{(\d+).*?\}", RegexOptions.Compiled);
        private static readonly Regex BrandRegex = new Regex(@"\$\{(\w+)\}", RegexOptions.Compiled);
        private const string ResourceFolder = "Resources";
        private const string ResourceFileExtension = ".resx";

        [Pure]
        private static bool CheckFormatParameters([CanBeNull] string defaultResourceString, [NotNull] string customResourceString)
        {
            if(defaultResourceString == null)
                return true;
            if (GetMaxFormatParameterNumber(defaultResourceString) != GetMaxFormatParameterNumber(customResourceString))
                return false;

            var defaultBrandParams = GetBrandFormatParameterInfo(defaultResourceString);
            var customBrandParams = GetBrandFormatParameterInfo(customResourceString);
            if (!defaultBrandParams.SequenceEqual(customBrandParams))
                return false;

            return true;
        }

        private static int GetMaxFormatParameterNumber(string str)
        {
            var max = -1;
            foreach (Match match in BracesCounterEx.Matches(str))
            {
                if (int.TryParse(match.Groups[1].Captures[0].Value, out var i))
                {
                    if (max < i)
                        max = i;
                }
            }
            return max;
        }

        [Pure]
        private static IEnumerable<(string key, int value)> GetBrandFormatParameterInfo([NotNull] string str)
        {
            var matches = new Dictionary<string, int>();
            foreach (Match match in BrandRegex.Matches(str))
            {
                var key = match.Groups[0].Value;
                if (!BrandInfo.ReplacementDict.ContainsKey(key))
                    Debug.Fail($"Wrong key: {key}");
                matches[key] = matches.GetOrDefault(key, 0) + 1;
            }

            return matches.Select(x => (key: x.Key, value: x.Value)).OrderBy(x => x.key);
        }

        [NotNull]
        private static string GetResxFileNameNetStyle([NotNull] string fileName, [NotNull] string cultureName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (cultureName == null)
                throw new ArgumentNullException(nameof(cultureName));

            var extension = Path.GetExtension(fileName);
            if (extension == null || !extension.Equals(ResourceFileExtension, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException(
                    $"{nameof(ResourceLoader)} supports only files with {ResourceFileExtension} extension, but file with extension \"{extension}\" was passed instead.",
                    nameof(fileName));
            }

            var localizedFileName = string.Format("{0}.{1}{2}", Path.GetFileNameWithoutExtension(fileName), cultureName, ResourceFileExtension);

            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ResourceFolder, localizedFileName);
        }

        [CanBeNull]
        private static IReadOnlyDictionary<string, string> LoadResourcesFromFile(string filePath, [NotNull] ResourceManager resourceManager)
        {
            try
            {
                using (var reader = new ResXResourceReader(filePath))
                {
                    var dataStringCache = new Dictionary<string, string>();
                    foreach (DictionaryEntry customResource in reader)
                    {
                        var resourceKey = (string)customResource.Key;
                        var resourceValue = customResource.Value;
                        if (!(resourceValue is string customResourceString))
                        {
                            var error = resourceValue == null
                                // Судя по экспериментам, null не может прийти в качестве значения.
                                // В работе ResXResourceReader обнаружен баг - при парсинге файла, содержащего данные в формате
                                // <data name="ResourceKey" xml:space="preserve" /> ридер "проскакивает" и захватывает значение следующего элемента.
                                // Так что неизвестно, что будет если в следующем сервис-паке это поведение поправят
                                ? $"ResourceLoader doesn't support null as resource value for {resourceKey} key in {filePath}"
                                : $"ResourceLoader doesn't support value of type {resourceValue.GetType()} for {resourceKey} key in {filePath}";

                            throw new RestoException(error);
                        }

                        customResourceString = NormalizeTabsAndLineBreaks(customResourceString);
                        var defaultResourceString = resourceManager.GetStringWithNormalizedTabsAndLineBreaks(resourceKey, CultureInfo.InvariantCulture);
                        // Защита от падений при несовпадении параметров строки-шаблона
                        // ReSharper disable once AssignNullToNotNullAttribute
                        var customResourceHasInvalidFormatParamsNumber = !CheckFormatParameters(defaultResourceString, customResourceString);
                        dataStringCache[resourceKey] = (customResourceHasInvalidFormatParamsNumber ? defaultResourceString : customResourceString)?.ToBrand();
                    }
                    return dataStringCache;
                }
            }
            catch (Exception ex)
            {
                LogWrapper.Log.Warn($"Can't load localization from file: \"{filePath}\"", ex);
                return null;
            }
        }

        #endregion Private Members

        #region Public Members

        /// <summary>
        /// Загружает для указанного ресурсного файла переопределённую языковую локализацию (если имеется).
        /// </summary>
        /// <param name="fileName">Имя ресурсного файла, включая расширение (.resx).</param>
        /// <param name="resourceManager">Менеджер ресурсов, созданный для указанного файла ресурсов.</param>
        [CanBeNull]
        public static IReadOnlyDictionary<string, string> LoadResources([NotNull] string fileName, [NotNull] ResourceManager resourceManager)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (resourceManager == null)
                throw new ArgumentNullException(nameof(resourceManager));

            var filePath = GetResxFileNameNetStyle(fileName, Localizer.Culture.Name);
            if (!File.Exists(filePath))
                return null;

            return LoadResourcesFromFile(filePath, resourceManager);
        }

        [Pure, CanBeNull]
        public static string GetStringWithNormalizedTabsAndLineBreaks([NotNull] this ResourceManager resourceManager, [NotNull] string name, [CanBeNull] CultureInfo culture)
        {
            if (resourceManager == null)
                throw new ArgumentNullException(nameof(resourceManager));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return NormalizeTabsAndLineBreaks(resourceManager.GetString(name, culture));
        }

        [Pure, ContractAnnotation("null => null")]
        private static string NormalizeTabsAndLineBreaks([CanBeNull] string source)
        {
            return source?.Replace("\\n", "\n").Replace("\\t", "\t");
        }

        [Pure, ContractAnnotation("null => null")]
        public static string ToBrand([CanBeNull] this string source)
        {
            if (source == null)
                return null;
            return BrandRegex.Replace(source,
                match =>
                {
                    if (!BrandInfo.ReplacementDict.TryGetValue(match.Groups[0].Value, out var result))
                        Debug.Fail($"Wrong key: {match.Groups[0].Value}");

                    return result ?? string.Empty;
                });
        }

        #endregion Public Members
    }
}