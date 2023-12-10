using System.Globalization;

namespace Resto.Common
{
    /// <summary>
    /// Расширенный класс для работы с культурой.
    /// </summary>
    public class CultureInfoEx
    {
        private readonly CultureInfo cultureInfo;
        private readonly string urlToDocumentation = string.Empty;
        private readonly bool showReleaseNotes;
        private readonly string language = string.Empty;
        private readonly string country = string.Empty;
        private readonly string speechName = string.Empty;

        public CultureInfoEx(string language, string country, CultureInfo cultureInfo, string urlToDocumentation, bool showReleaseNotes, string speechName)
        {
            this.language = language;
            this.country = country;
            this.cultureInfo = cultureInfo;
            this.urlToDocumentation = urlToDocumentation;
            this.showReleaseNotes = showReleaseNotes;
            this.speechName = speechName;
        }

        /// <summary>
        /// Ссылка на документацию.
        /// </summary>
        public string UrlToDocumentation
        {
            get { return urlToDocumentation; }
        }

        /// <summary>
        /// Возможен просмотр Release Notes.
        /// </summary>
        public bool ShowReleaseNotes
        {
            get { return showReleaseNotes; }
        }

        /// <summary>
        /// Возвращает ифнормацию по культуре.
        /// </summary>
        public CultureInfo CultureInfo
        {
            get { return cultureInfo; }
        }

        /// <summary>
        /// Имя языка.
        /// </summary>
        public string Language
        {
            get { return language; }
        }

        /// <summary>
        /// Регион языка.
        /// </summary>
        public string Country
        {
            get { return country; }
        }

        /// <summary>
        /// Название языка на этом же языке.
        /// </summary>
        public string SpeechName
        {
            get { return speechName; }
        }
    }
}
