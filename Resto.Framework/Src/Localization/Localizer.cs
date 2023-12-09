using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

using Resto.Framework.Attributes.JetBrains;

using Resto.Framework.Common;

namespace Resto.Framework.Localization
{
    /// <summary>
    /// Класс, предоставляющий доступ к ресурсам модуля.
    /// Данные получаются из встроенных ресурсов и уточняются с помощью файлов
    /// языковой и отраслевой локализаций.
    /// </summary>
    public sealed class Localizer
    {
        #region Static Members

        #region Static Fields

        // NOTE: Инициализация поля значением по умолчанию необходима для параметризованных юнит-тестов,
        //       где вычисление параметров (через атрибуты Values, ValueSource и т. п.) обращается к 
        //       локализованным ресурсам до вызова метода SetUp для тестируемого метода/класса/сборки.
        //
        //       По-хорошему, инициализация для тестов должна делаться только в тестах, но для этого надо
        //       как-то вклиниться в вычисление параметров NUnit (через addin?) или ограничиться простыми
        //       значениями параметризованных аргументов.
        //       Сейчас там есть и прямые обращения к локализованным ресурсам, и использование java-enum'ов
        //       с локализуемым названием (например, Permission), для которых NUnit вызывает ToString().
        private static volatile CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");
        private static readonly HashSet<Localizer> LoadedLocalizers = new HashSet<Localizer>();
        /// <summary>
        /// Объект для синхронизации при работе со списком загруженных локализаторов.
        /// </summary>
        private static readonly object LocalizerListSync = new object();

        #endregion Static Fields

        #region Static Properties

        /// <summary>
        /// Возвращает или устанавливает настройки локализации.
        /// </summary>
        /// <remarks>
        /// После установки настроек локализации для всех зарегистрированных локализаторов производится перегрузка ресурсов.
        /// </remarks>
        /// <exception cref="Resto.Framework.Common.RestoException">
        /// В случае попытки получения значения свойства до его первой установки, выбрасывается исключение.
        /// </exception>
        [NotNull]
        public static CultureInfo Culture
        {
            get
            {
                if (culture == null)
                    throw new RestoException("Localizer's culture wasn't set. Use \"Localizer.Culture\" property to set localization culture before using it.");

                return culture;
            }
            set
            {
                var oldCulture = culture;
                if (value.Equals(oldCulture) && value.DateTimeFormat.Equals(oldCulture.DateTimeFormat))
                    return;

                // Используем оптимистическую блокировку.
#pragma warning disable 420
                // http://msdn.microsoft.com/en-us/library/4bw5ewxy(VS.80).aspx
                Interlocked.CompareExchange(ref culture, value, oldCulture);
#pragma warning restore 420

                ReloadAll();
            }
        }

        /// <summary>
        /// Возвращает <c>true</c> если настройки локализации <see cref="Culture"/> были установлены, иначе - <c>false</c>.
        /// </summary>
        /// <seealso cref="Culture" />
        public static bool IsConfigured => culture != null;

        #endregion Static Properties

        #region Static Methods

        /// <summary>
        /// Создает локализатор для указанного ресурсного файла.
        /// </summary>
        /// <param name="fileName">Имя ресурсного файла, для которого создается локализатор.</param>
        /// <param name="rmNamespace">Полный "ресурсный путь" к файлу (без расширения) для доступа к нему через менеджер ресурсов.</param>
        /// <param name="assembly">Сборка, содержащая файл в качестве встроенных ресурсов.</param>
        /// <returns>Локализатор, позволяющий получать по строковому ключу локализованные данные.</returns>
        public static Localizer Create([NotNull] string fileName, [NotNull] string rmNamespace, [NotNull] Assembly assembly)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (rmNamespace == null)
                throw new ArgumentNullException(nameof(rmNamespace));
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            var localizer = new Localizer(fileName, rmNamespace, assembly);
            lock (LocalizerListSync)
            {
                if (!LoadedLocalizers.Contains(localizer))
                {
                    localizer.Reload();
                    LoadedLocalizers.Add(localizer);
                }
            }
            return localizer;
        }

        private static void ReloadAll()
        {
            lock (LocalizerListSync)
            {
                foreach (var localizer in LoadedLocalizers)
                {
                    localizer.Reload();
                }
            }
        }

        #endregion Static Methods

        #endregion Static Members

        #region Fields

        private readonly string rsxFileName;
        private volatile IReadOnlyDictionary<string, string> dataStringCache = new Dictionary<string, string>();
        private readonly ResourceManager resourceManager;
        /// <summary>
        /// Хэш-код сборки, которая используется при создании объекта. Используется для подстраховки от коллизий.
        /// </summary>
        private readonly int assemblyHashCode;
        /// <summary>
        /// Хэш-код объекта, рассчитываемый при создании и не меняющийся в дальнейшем, так как рассчитывается по readonly-полям.
        /// </summary>
        private readonly int calculatedHashCode;

        #endregion Fields

        #region Constructors

        private Localizer(string fileName, string rmNamespace, Assembly assembly)
        {
            rsxFileName = fileName;
            resourceManager = new ResourceManager(rmNamespace, assembly);
            assemblyHashCode = assembly.GetHashCode();
            calculatedHashCode = fileName.GetHashCode() ^ rmNamespace.GetHashCode() ^ assemblyHashCode;
        }

        #endregion Constructors

        #region Methods

        private void Reload()
        {
            var newResources = ResourceLoader.LoadResources(rsxFileName, resourceManager);
            if (newResources == null)
                return;

            dataStringCache = newResources;
        }

        /// <summary>
        /// Возвращает локализованное значение строкового ресурса по указанному ключу.
        /// </summary>
        /// <param name="code">Ключ строкового ресурса.</param>
        /// <returns>Локализованное значение строкового ресурса.</returns>
        [NotNull]
        public string GetStringFromResources([NotNull] string code)
        {
            var result = TryGetStringFromResources(code);
            if (result == null)
                throw new RestoException($"String resource with key \"{code}\" is not registered");

            return result;
        }

        [CanBeNull]
        public string TryGetStringFromResources([NotNull] string code)
        {
            return dataStringCache.TryGetValue(code, out var cachedResource)
                ? cachedResource
                : resourceManager.GetStringWithNormalizedTabsAndLineBreaks(code, Culture).ToBrand();
        }
        #endregion Methods

        #region Overridden Methods

        public override bool Equals(object obj)
        {
            return obj is Localizer other
                && GetHashCode() == other.GetHashCode()
                && assemblyHashCode == other.assemblyHashCode
                && string.Equals(rsxFileName, other.rsxFileName, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(resourceManager.BaseName, other.resourceManager.BaseName, StringComparison.InvariantCulture);
        }

        public override int GetHashCode()
        {
            return calculatedHashCode;
        }

        #endregion Overridden Methods
    }
}