using System;
using System.Collections.Generic;
using System.Globalization;
using Resto.Data;
using Resto.Framework.Common;
using log4net;
using Resto.Framework.Localization;

namespace Resto.Common
{
    ///<summary>
    /// Класс-помошник для работы с культурами.
    ///</summary>
    public static class LocalizationHelper
    {

        private static readonly ILog LOG = LogFactory.Instance.GetLogger(typeof(LocalizationHelper));
        private static readonly Dictionary<string, CultureInfoEx> cultures = new Dictionary<string, CultureInfoEx>();

        static LocalizationHelper()
        {
            var languages = RestoLocale.VALUES;
            string urlDoc = string.Empty;

            foreach (var speech in languages)
            {
                if (speech.EnabledInOffice)
                {
                    //если есть ссылка на документацию для данного языка приложения
                    if (!speech.UrlToDocumentation.IsNullOrEmpty())
                    {
                        Register(speech.GetCultureName(), speech.UrlToDocumentation, speech.ShowReleaseNotes, speech.Language, speech.Country, speech.Name);
                    }
                    else
                    {
                        //если нет значения дефолтного языка, то берем  ссылку на документацию для русского языка
                        if (speech.DefaultLanguageCode.IsNullOrEmpty())
                        {
                            Register(speech.GetCultureName(), RestoLocale.RU_RU.UrlToDocumentation, speech.ShowReleaseNotes, speech.Language, speech.Country, speech.Name);
                        }
                        //если дефолтный язык указан, то ссылку на документацию для данного языка берем у него
                        else
                        {
                            urlDoc = string.Empty;
                            foreach (var word in languages)
                            {
                                if (word.Code.Equals(speech.DefaultLanguageCode))
                                {
                                    urlDoc = word.UrlToDocumentation;
                                }
                            }
                            Register(speech.GetCultureName(), urlDoc, speech.ShowReleaseNotes, speech.Language, speech.Country, speech.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Регистрация поддерживаемых культур.
        /// </summary>
        private static void Register(string cultureName, string urlToDocumentation, bool showReleaseNotes, string language, string country, string fullLanguageName)
        {
            try
            {
                CultureInfo cultureInfo = CultureInfo.GetCultureInfo(cultureName);
                cultures.Add(cultureName, new CultureInfoEx(language, country, cultureInfo, urlToDocumentation, showReleaseNotes, fullLanguageName));
            }
            catch (Exception ex)
            {
                LOG.Error($"Culture not register: {cultureName}. {ex}.");
            }
        }

        /// <summary>
        /// Показывать Release Notes для данного языка приложения.
        /// </summary>
        public static bool ShowReleaseNotes
        {
            get
            {
                // TODO RMS-53515 Release Notes отключены для некоторых брендов, нужно включить когда будет ясно, что делать
                if (!BrandInfo.ShowRealeaseNotesInOffice)
                {
                    return false;
                }

                return IsRegistered(CultureName) && cultures[CultureName].ShowReleaseNotes;
            }
        }

        /// <summary>
        /// Есть ли ссылка на документацию для данного языка приложения.
        /// </summary>
        public static bool HasUrlToDocumentation
        {
            get
            {
                // TODO RMS-53515 документация отключена для некоторых брендов, нужно включить когда будет ясно, что делать
                if (!BrandInfo.ShowDocumentationInOffice)
                {
                    return false;
                }

                return IsRegistered(CultureName) && !string.IsNullOrEmpty(cultures[CultureName].UrlToDocumentation);
            }
        }

        /// <summary>
        /// Ссылка на документацию для данного языка приложения.
        /// </summary>
        public static string UrlToDocumentation
        {
            get
            {
                return HasUrlToDocumentation ? cultures[CultureName].UrlToDocumentation : string.Empty;
            }
        }

        /// <summary>
        /// Возвращает информацию о культуре.
        /// Если культура не зарегистрированна в cultures.xml, то по умолчанию возвращаем en-us.
        /// </summary>
        public static CultureInfo GetCulture(string name)
        {
            if (!IsRegistered(name))
            {
                return cultures[RestoLocale.EN_US.GetCultureName()].CultureInfo;
            }
            return cultures[name].CultureInfo;
        }

        /// <summary>
        /// Возвращает true, если имя равняется текущей культуре.
        /// </summary>
        public static bool IsCurrentCulture(RestoLocale name)
        {
            return Localizer.IsConfigured && CultureName == name.GetCultureName();
        }

        /// <summary>
        /// Возвращает true, если имя равняется текущей культуре.
        /// </summary>
        public static bool IsCurrentCulture(string name)
        {
            return Localizer.IsConfigured && CultureName == name;
        }

        private static bool IsRegistered(string name)
        {
            return cultures.ContainsKey(name);
        }

        public static Dictionary<string, CultureInfoEx> GetCultures()
        {
            return cultures;
        }

        /// <summary>
        /// Устанавливает культуру по данным конфигурационного файла (%AppData%\iiko\Rms\config\backclient.config.xml.
        /// </summary>
        public static void AssignConfigBasedCulture()
        {
            //Для арабарской локали необходимо использовать григорианский кадендарь.
            Localizer.Culture = CommonConfig.Instance.UiCultureName == "ar-SA"
                ? new GregorianArabicCulture()
                : CultureInfo.CreateSpecificCulture(CommonConfig.Instance.UiCultureName);
        }

        /// <summary>
        /// Возвращает имя текущей культуры или пустую строку.
        /// </summary>
        public static string CultureName
        {
            get
            {
                return Localizer.IsConfigured ? Localizer.Culture.Name : string.Empty;
            }
        }
    }
}
