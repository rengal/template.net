using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;
using log4net;
using Resto.Framework.Common;
using Resto.Framework.Properties;

namespace Resto.Framework
{
    public static class HelpHelper
    {
        #region Private fields

        /// <summary>
        /// Невидимая форма для обхода дефекта c окном помощи.
        /// </summary>
        /// <remarks>
        /// http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/ce1e8382-b2a9-4985-a958-4a8d5e944738
        /// </remarks>
        private static readonly Lazy<Form> HelpParent = new Lazy<Form>(() => new Form());

        /// <summary>
        /// Содержит имя файла справки.
        /// </summary>
        private static Lazy<string> helpUrl;

        /// <summary>
        /// Текущий язык документации. Определяется на основе CurrentUICulture 
        /// </summary>
        private static Lazy<string> documentationLanguage;

        /// <summary>
        /// Путь к файлу releaseNotes
        /// </summary>
        private static Lazy<string> releaseNotesPath;

        /// <summary>
        /// Логгер.
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(HelpHelper));

        /// <summary>
        /// Идентификатор продукта, который будет использоваться "по умолчанию" при открытии справки.
        /// </summary>
        private static string defaultProduct;

        private static readonly Dictionary<int, (string product, int? helpIdx)> CustomHelpIndexMap = new Dictionary<int, (string product, int? helpIdx)>();
        #endregion

        #region Private constants

        /// <summary>
        /// Содержит указатель на стартовую страницу help продукта
        /// </summary>
        private const string DefaultPage = "getting-started";

        #endregion

        #region Public methods

        /// <summary>
        /// Инициализирует класс исходными данными для отображения справки (online/offline) для правильной версии и на нужном языке.
        /// </summary>
        /// <param name="uriTemplate">Шаблон ссылки на раздел справки. Ссылка может быть и в виде uri (file://, http://?, https://) и в виде прочтого пути к файлу (c:\path\{topic}.html). При раскрытии шаблона используются плейсхолдеры {topic} (id раздела справки), {edition} (id продукта), 
        /// {version} (версия продукта в формате x.x), {language} (язык справки, ru, en и т.д.)</param>
        /// <param name="translations">Список поддерживаемых переводов справки (en;ru;...), первый в списке - дефолтный, используется когда текущая культура приложения не входит в этот список</param>
        /// <param name="productId">символьный идентификатор продукта, используется для подстановки в шаблон адреса (плейсхолдер {product} )</param>
        /// <param name="docsVersion">версия документации, используется для подстановки в шаблон адреса (плейсходеры {version}/{version:-}/{version:_}) </param>
        public static void Initialize([NotNull] string uriTemplate, [NotNull] ICollection<string> translations, [NotNull] string productId, [NotNull] Version docsVersion)
        {
            if (uriTemplate == null)
            {
                throw new ArgumentOutOfRangeException(nameof(uriTemplate),
                    string.Format(Resources.HelpFileNameIsNull, uriTemplate));
            }
            if (translations == null)
            {
                throw new ArgumentNullException(nameof(translations));
            }
            if (docsVersion == null)
            {
                throw new ArgumentNullException(nameof(docsVersion));
            }

            defaultProduct = productId ?? throw new ArgumentNullException(nameof(productId));

            helpUrl = new Lazy<string>(() =>
            {
                var placeholders = new Hashtable()
                {
                    ["topic"] = "{1}",
                    ["product"] = "{0}",
                    ["language"] = "{2}",
                    ["version"] = docsVersion.ToString(2),
                    ["version:-"] = docsVersion.ToString(2).Replace('.', '-'),
                    ["version:_"] = docsVersion.ToString(2).Replace('.', '_'),
                };


                const string pattern = @"\{[\w:-]*\}";
                var regex = new Regex(pattern, RegexOptions.Compiled);

                return regex.Replace(uriTemplate, match =>
                {
                    var name = match.Value.Trim('{', '}');
                    return placeholders.Contains(name) ? placeholders[name].ToString() : string.Empty;
                });
            });

            documentationLanguage = new Lazy<string>(() =>
            {
                var userLanguage = translations.FirstOrDefault(IsCurrentLanguage) ?? translations.FirstOrDefault();

                //проверяем является ли HelpUrl валидной ссылкой на веб-сайт
                if (Uri.TryCreate(FormatHelpUrl(helpUrl.Value, defaultProduct, DefaultPage, userLanguage), UriKind.Absolute, out Uri documentationUri)
                    && (documentationUri.Scheme == Uri.UriSchemeHttp || documentationUri.Scheme == Uri.UriSchemeHttps))
                {
                    //Документация размещена на сайте
                    return userLanguage;
                }

                //Локальная документация. Будет возвращен первый язык из списка (translations) для которого имеется документация (DefaultPage).
                //Текущий язык UI проверяется первым.
                return translations
                        .Prepend(userLanguage)
                        .FirstOrDefault(lang =>
                            File.Exists(FormatHelpUrl(helpUrl.Value, defaultProduct, DefaultPage, lang)));
            });

            releaseNotesPath = new Lazy<string>(() =>
            {
                const string releaseNotesFileNameTemplate = "readme.{0}.mht";

                var root = Path.GetDirectoryName(Application.StartupPath);
                if (root == null) return string.Empty;

                var filePathTemplate = Path.Combine(root, releaseNotesFileNameTemplate);

                //Определяем существуествует ли файл releaseNotes для текущего языка UI.
                //Если нет - будет использован первый имеющийся файл (из списка языков translations)
                var targetPath = translations
                        .Prepend(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                        .Select(lang => string.Format(filePathTemplate, lang))
                        .FirstOrDefault(File.Exists);

                return targetPath ?? string.Format(filePathTemplate, documentationLanguage.Value);
            });

        }

        /// <summary>
        /// Отображает заданный раздел справки приложения.
        /// </summary>
        /// <param name="topicId">идентификатор раздела справки</param>
        public static void ShowHelp(int? topicId = null)
        {
            // Значение флага BrandInfo.ShowDocumentationInOffice генерируемое -
            // для управления флагом следует внести изменения в файл настроек для бренда \Resources\Brand\ИМЯ_БРЕНДА\BrandInfo.resx
            if (!BrandInfo.ShowDocumentationInOffice)
            {
#pragma warning disable CS0162 // Unreachable code detected
                return;
#pragma warning restore CS0162 
            }

            if (helpUrl == null)
            {
                throw new InvalidOperationException(Resources.HelpHelperClassMustBeInitialize);
            }

            if (documentationLanguage?.Value == null)
            {
                throw new InvalidOperationException(Resources.HelpHelperNoAnyDocumentation);
            }

            var index = topicId.HasValue && CustomHelpIndexMap.TryGetValue(topicId.Value, out var idx)
                ? idx
                : (product: defaultProduct, helpIdx: topicId);
            var topic = index.helpIdx.HasValue ? $"topic-{index.helpIdx}" : DefaultPage;
            try
            {
                Help.ShowHelp(HelpParent.Value, FormatHelpUrl(helpUrl.Value, index.product, topic, documentationLanguage.Value));
            }
            catch (Exception e)
            {
                Log.Warn(e);
                Help.ShowHelp(HelpParent.Value, FormatHelpUrl(helpUrl.Value, index.product, DefaultPage, documentationLanguage.Value));
            }
        }

        /// <summary>
        /// Возвращает полный путь к файлу релиз-нот для текущего языка UI. Если файл отсутствует - то возвращает путь к первому имеющемуся файлу релиз-нот.
        /// Ожидается, что релиз-ноты имеются хотя бы для одного языка.
        /// </summary>
        public static string ReleaseNotesPath
        {
            get
            {
                if (releaseNotesPath == null)
                    throw new InvalidOperationException(Resources.HelpHelperClassMustBeInitialize);
                return releaseNotesPath.Value;
            }
        }
        

        public static void RegisterProductHelpScope(string productScope, IDictionary<int, int?> globalIndexMapping)
        {
            if (string.IsNullOrEmpty(productScope))
            {
                throw new ArgumentException("Argument is null or empty", nameof(productScope));
            }

            if (globalIndexMapping == null)
            {
                throw new ArgumentNullException(nameof(globalIndexMapping));
            }

            CustomHelpIndexMap.AddRange(globalIndexMapping.ToDictionary(item => item.Key, item => (productScope, item.Value)));
        }
        #endregion

        #region Private methods

        private static  bool IsCurrentLanguage(string culture)
        {
            return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals(CultureInfo.GetCultureInfo(culture).TwoLetterISOLanguageName);
        }

        private static string FormatHelpUrl(string urlTemplate, string productId, string topic, string language)
        {
            return string.Format(urlTemplate, productId, topic, language);
        }

        #endregion
    }
}