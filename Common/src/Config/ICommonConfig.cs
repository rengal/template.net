using System;
using System.Net;

namespace Resto.Common
{
    /// <summary>
    /// Настройки конфига используемые в OfficeCommon.
    /// Используется для предотвращения создания файла конфигурации другими клиентами, которым нужен проект OfficeCommon.
    /// </summary>
    public interface ICommonConfig
    {
        /// <summary>
        /// Настройка, проверять конвертируемый/сериализуемый объекта на наличие в кэше.
        /// </summary>
        bool CheckObjectInCacheForConverter { get; set; }

        /// <summary>
        /// Путь до папки хранения настроек.
        /// </summary>
        string HomePath { get; }

        /// <summary>
        /// Позволяет установить значение для <see cref="HttpWebRequest.KeepAlive"/> флагов в проекте.
        /// </summary>
        bool KeepAliveHttpWebRequestFlag { get; set; }

        /// <summary>
        /// Начальный путь к реестру для приложения.
        /// </summary>
        string MainRegKey { get; }

        /// <summary>
        /// Подставлять цену по последнему приходу.
        /// </summary>
        bool NeedSubstituteSupplierPrice { get; set; }

        /// <summary>
        /// Кол-во доп-х обращений к серверу, при не удачной попытке.
        /// </summary>
        int RepeatCallsServerCount { get; set; }

        /// <summary>
        /// Инвервал ожидания между попытками обращения к серверу в случае если предыдущая завершилась не удачно.
        /// </summary>
        int RepeatCallsServerTimeoutInMs { get; set; }

        /// <summary>
        /// Путь к серверу.
        /// </summary>
        string ServerUrl { get; }

        /// <summary>
        /// Имя культуры, с учётом выбранного языка приложения.
        /// </summary>
        string UiCultureName { get; set; }

        /// <summary>
        /// Использовать товары поставщика.
        /// </summary>
        bool UseSupplierProducts { get; set; }

        /// <summary>
        /// Настройка, обновлять кэш бэка после переподключения сервера.
        /// </summary>
        bool UpdateCacheAfterReconnection { get; set; }

        /// <summary>
        /// Ежедневное сохранение кэша по окончании рабочего дня.
        /// </summary>
        bool DailyCacheSaving { get; set; }

        /// <summary>
        /// Период хранения ежедневного кэша.
        /// Настройка так же распространяется на папку ТП,
        /// для которого хранятся кэш и архивы.
        /// </summary>
        int CacheSavingPeriod { get; set; }

        /// <summary>
        /// Время для ежедневного сохранения кэша.
        /// </summary>
        /// <remarks>Хранится в минутах</remarks>
        int CacheSavingMinutesTime { get; set; }

        /// <summary>
        /// Возможность архивирования кэша.
        /// </summary>
        bool CanArchiveCache { get; set; }

        /// <summary>
        /// Сохраняет конфиг.
        /// </summary>
        bool Save();
    }
}
