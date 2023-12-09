// This file was generated with XmlToCodeGenerator.
// Do not edit it manually.


namespace Resto.Front.Localization.Resources
{
    public static partial class EmergencyOperationsLocalResources
    {
        private static readonly Resto.Framework.Localization.Localizer Localizer = Resto.Framework.Localization.Localizer.Create("EmergencyOperationsLocalResources.resx", "Resto.Front.Localization.Resources.EmergencyOperationsLocalResources", System.Reflection.Assembly.GetExecutingAssembly());

        /// <summary>
        ///   Looks up a localized string similar to "Учет рабочего времени"
        /// </summary>
        public static System.String SwitchSendAttendancesInfoToServerFlagMessageTitle
        {
            get { return Localizer.GetStringFromResources("SwitchSendAttendancesInfoToServerFlagMessageTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Данный терминал будет использоваться для учета рабочего времени.<br />
        /// Если в этом режиме пользователь открывает или закрывает личную смену<br />
        /// на данном терминале, изменения сохраняются на сервере.<br />
        /// <br />
        /// Эти личные смены будут участвовать в формировании явок, учете отработанного<br />
        /// времени, расчете заработной платы и т.п.<br />
        /// <br />
        /// При включении режима все личные смены, открытые ранее на данном терминале,<br />
        /// будут автоматически закрыты. Эти личные смены не сохранятся на сервере.<br />
        /// <br />
        /// Чтобы включить учет рабочего времени, прокатайте карту пользователя с правом<br />
        /// «F_KIS Принудительно закрывать личные смены»."
        /// </summary>
        public static System.String SwitchSendAttendancesInfoToServerOnMessage
        {
            get { return Localizer.GetStringFromResources("SwitchSendAttendancesInfoToServerOnMessage"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Данный терминал не будет использоваться для учета рабочего времени.<br />
        /// Если в этом режиме пользователь открывает или закрывает личную смену<br />
        /// на данном терминале, изменения не сохраняются на сервере.<br />
        /// <br />
        /// Эти личные смены не будут участвовать в формировании явок, учете отработанного<br />
        /// времени, расчете заработной платы и т.п.<br />
        /// <br />
        /// При выключении режима все личные смены, открытые ранее на данном терминале,<br />
        /// будут автоматически закрыты. Эти личные смены сохранятся на сервере.<br />
        /// <br />
        /// Чтобы отключить учет рабочего времени, прокатайте карту пользователя с правом<br />
        /// «F_KIS Принудительно закрывать личные смены»."
        /// </summary>
        public static System.String SwitchSendAttendancesInfoToServerOffMessage
        {
            get { return Localizer.GetStringFromResources("SwitchSendAttendancesInfoToServerOffMessage"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Использовать терминал для учета рабочего времени"
        /// </summary>
        public static System.String SwitchSendAttendancesInfoToServerFlagOnButtonText
        {
            get { return Localizer.GetStringFromResources("SwitchSendAttendancesInfoToServerFlagOnButtonText"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не использовать терминал для учета рабочего времени"
        /// </summary>
        public static System.String SwitchSendAttendancesInfoToServerFlagOffButtonText
        {
            get { return Localizer.GetStringFromResources("SwitchSendAttendancesInfoToServerFlagOffButtonText"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Ручной сброс блокировок"
        /// </summary>
        public static System.String ManualResetLocks
        {
            get { return Localizer.GetStringFromResources("ManualResetLocks"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Закрытие кассовой смены"
        /// </summary>
        public static System.String CafeSessionCloseOperationName
        {
            get { return Localizer.GetStringFromResources("CafeSessionCloseOperationName"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Адрес"
        /// </summary>
        public static System.String AgentAddressColumnHeader
        {
            get { return Localizer.GetStringFromResources("AgentAddressColumnHeader"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Заблокированных объектов нет"
        /// </summary>
        public static System.String LocksListIsEmptyText
        {
            get { return Localizer.GetStringFromResources("LocksListIsEmptyText"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Выберите терминал, чтобы сбросить его блокировки"
        /// </summary>
        public static System.String ManualResetLocksHeader
        {
            get { return Localizer.GetStringFromResources("ManualResetLocksHeader"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "При выходе из строя терминала некоторые объекты могут остаться заблокированными и быть недоступными на других терминалах.<br />
        /// В этом случае можно применить ручной сброс блокировок."
        /// </summary>
        public static System.String ManualResetLocksHint
        {
            get { return Localizer.GetStringFromResources("ManualResetLocksHint"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Терминал"
        /// </summary>
        public static System.String TerminalNameColumnHeader
        {
            get { return Localizer.GetStringFromResources("TerminalNameColumnHeader"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Банкет №{0} на стол {1} ({2}) на {3}"
        /// </summary>
        public static System.String LockedBanquetDescriptionFormat
        {
            get { return Localizer.GetStringFromResources("LockedBanquetDescriptionFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Доставка №{0} на {1} для клиента {2}"
        /// </summary>
        public static System.String LockedDeliveryDescriptionFormat
        {
            get { return Localizer.GetStringFromResources("LockedDeliveryDescriptionFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Документ №{0} «{1}»"
        /// </summary>
        public static System.String LockedDocumentDescriptionFormat
        {
            get { return Localizer.GetStringFromResources("LockedDocumentDescriptionFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Кол-во"
        /// </summary>
        public static System.String LockedEntitiesCountColumnHeader
        {
            get { return Localizer.GetStringFromResources("LockedEntitiesCountColumnHeader"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Заблокированные объекты"
        /// </summary>
        public static System.String LockedEntityDescriptionsColumnHeader
        {
            get { return Localizer.GetStringFromResources("LockedEntityDescriptionsColumnHeader"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Заказ №{0} на столе {1} ({2})"
        /// </summary>
        public static System.String LockedOrderDescriptionFormat
        {
            get { return Localizer.GetStringFromResources("LockedOrderDescriptionFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Резерв №{0} на стол {1} ({2}) на {3}"
        /// </summary>
        public static System.String LockedReserveDescriptionFormat
        {
            get { return Localizer.GetStringFromResources("LockedReserveDescriptionFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Будьте осторожны!<br />
        /// Принудительный сброс блокировок может нарушить работу системы и привести к потере данных.<br />
        /// Безопасный способ – нажать «Отмена» и корректно завершить работу с заблокированными объектами.<br />
        /// Если вы сомневаетесь, обратитесь к системному администратору или в службу техподдержки.<br />
        /// Продолжайте, только если вы уверены в необходимости этой операции<br />
        /// и понимаете, к каким последствиям она может привести.<br />
        /// <br />
        /// Сбросить блокировки терминала «{0}»?"
        /// </summary>
        public static System.String ManualResetLocksConfirmation
        {
            get { return Localizer.GetStringFromResources("ManualResetLocksConfirmation"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "(главная касса)"
        /// </summary>
        public static System.String TerminalIsMainCashHint
        {
            get { return Localizer.GetStringFromResources("TerminalIsMainCashHint"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Восстановление данных с главной кассы"
        /// </summary>
        public static System.String ResetTerminalSynchronization
        {
            get { return Localizer.GetStringFromResources("ResetTerminalSynchronization"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Будьте осторожны!<br />
        /// Кассовые данные (заказы, документы, доставки и т.д.) будут загружены с главной кассы.<br />
        /// Если введенные на терминале данные отсутствуют на главной кассе, то они будут утеряны.<br />
        /// Продолжить?"
        /// </summary>
        public static System.String ResetTerminalSynchronizationConfirmation
        {
            get { return Localizer.GetStringFromResources("ResetTerminalSynchronizationConfirmation"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Внимание!<br />
        /// Кассовые данные устарели, для их восстановления необходима перезагрузка.<br />
        /// Кассовые данные (заказы, документы, доставки и т.д.) будут загружены с главной кассы.<br />
        /// Если введенные на терминале данные отсутствуют на главной кассе, то они будут утеряны."
        /// </summary>
        public static System.String TerminalSynchronizationResetRequired
        {
            get { return Localizer.GetStringFromResources("TerminalSynchronizationResetRequired"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Внимание!<br />
        /// Обновлены настройки главной кассы, для их применения необходима перезагрузка."
        /// </summary>
        public static System.String ApplicationRestartRequiredDueToMainCashChange
        {
            get { return Localizer.GetStringFromResources("ApplicationRestartRequiredDueToMainCashChange"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "{0}<br />
        /// <br />
        /// Автоматическая перезагрузка будет выполнена через {1} секунд."
        /// </summary>
        public static System.String AutoRestartWarningMessage
        {
            get { return Localizer.GetStringFromResources("AutoRestartWarningMessage"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Внимание!<br />
        /// Кассовые данные повреждены. Для продолжения работы необходима перезагрузка.<br />
        /// Некоторые кассовые данные (заказы, документы и т.д.) будут утеряны."
        /// </summary>
        public static System.String MainCashSynchronizationResetRequired
        {
            get { return Localizer.GetStringFromResources("MainCashSynchronizationResetRequired"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Для закрытия кассовой смены необходимо закрыть все оплаченные доставки."
        /// </summary>
        public static System.String ForCloseCafeSessionNeedClosePaidDeliveries
        {
            get { return Localizer.GetStringFromResources("ForCloseCafeSessionNeedClosePaidDeliveries"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Для закрытия кассовой смены необходимо закрыть все доставки, обещанное время приготовления которых меньше времени окончания предыдущего учетного дня."
        /// </summary>
        public static System.String ForCloseCafeSessionNeedCloseOutdatedYesterdayDeliveries
        {
            get { return Localizer.GetStringFromResources("ForCloseCafeSessionNeedCloseOutdatedYesterdayDeliveries"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Для закрытия кассовой смены необходимо закрыть все доставки с фискальной предоплатой."
        /// </summary>
        public static System.String ForCloseCafeSessionNeedClosePrepaidDeliveries
        {
            get { return Localizer.GetStringFromResources("ForCloseCafeSessionNeedClosePrepaidDeliveries"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Для закрытия кассовой смены необходимо закрыть все доставки, обещанное время приготовления которых меньше времени окончания текущего учетного дня."
        /// </summary>
        public static System.String ForCloseCafeSessionNeedCloseOutdatedTodayDeliveries
        {
            get { return Localizer.GetStringFromResources("ForCloseCafeSessionNeedCloseOutdatedTodayDeliveries"); }
        }

    }
}

