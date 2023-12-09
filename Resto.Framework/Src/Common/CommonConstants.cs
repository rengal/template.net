using System;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Магические константы.
    /// </summary>
    public static class CommonConstants
    {
        #region Front Common
        /// <summary>
        /// Максимальная длина комментариев во фронте (к удалению блюда, внесению/изъятию, к явкам и т.п.).
        /// </summary>
        public const int MaxCommentLength = 255;

        /// <summary>
        /// Максимальная длина комментария к заказу.
        /// </summary>
        public const int MaxOrderCommentLength = MaxCommentLength;

        /// <summary>
        /// Точность по-умолчанию для цифрового нумпада.
        /// </summary>
        public const int DefaultDecimalPrecision = 9;

        /// <summary>
        /// Максимальная длина кода подключения к серверу.
        /// </summary>
        public const int MaxLengthOfConnectionCode = 6;

        /// <summary>
        /// Название улицы по умолчанию.
        /// </summary>
        public const string EmptyStreetName = "----------";

        /// <summary>
        /// Номер дома по умолчанию для обратной совместимости со старым форматом.
        /// </summary>
        public const string EmptyHouseNumber = "-";
        #endregion

        #region Order
        /// <summary>
        /// Количество, устанавливаемое по умолчанию при добавлении нового элемента в заказ
        /// </summary>
        public const decimal InitialItemAmount = 1m;

        /// <summary>
        /// Минимальное количество элемента в заказе.
        /// </summary>
        public const decimal MinimumItemAmount = 0.001m;

        /// <summary>
        /// Максимальное количество элемента в заказе.
        /// </summary>
        public const decimal MaximumItemAmount = 999.999m;

        /// <summary>
        /// Максимально количество суммы элемента в заказе
        /// </summary>
        public const decimal MaximumDocumentAmount = 100000000m;

        /// <summary>
        /// Максимальное количество модификатора у блюда в заказе.
        /// </summary>
        public const int MaximumModifierAmount = 99;

        /// <summary>
        /// Максимальное количество оплачиваемой по времени услуги в заказе (в минутах, 12 часов)
        /// </summary>
        public const decimal MaximumTimePayProductAmount = 12m * 60m;

        /// <summary>
        /// Максимальная продолжительность оплачиваемой по времени услуги в заказе
        /// </summary>
        public static readonly TimeSpan MaximumTimePayServiceDuration = new TimeSpan(12, 0, 0);

        /// <summary>
        /// Номер ячейки официанта, передаваемый при создании заказа, чтобы заказ добавился в первую свободную ячейку.
        /// </summary>
        public const int FirstFreeWaiterSlot = -1;

        /// <summary>
        /// Минимальный номер ячейки официанта
        /// </summary>
        public const int MinWaiterSlot = 0;

        /// <summary>
        /// Максимальный номер ячейки официанта
        /// </summary>
        public const int MaxWaiterSlot = 15;

        /// <summary>
        /// Максимальное количество гостей (OrderItemGuestModel) в заказе. При достижении максимума добавление гостей становится невозможным.
        /// </summary>
        public const int MaxGuestModelCount = 50;

        /// <summary>
        /// Максимально количество гостей (живых людей) в заказе.
        /// </summary>
        public const int MaxRealGuestCount = 1000;

        /// <summary>
        /// Количество гостей по умолчанию для нового заказа.
        /// </summary>
        public const int DefaultGuestsCount = 1;

        /// <summary>
        /// Промежуток времени, по прохождении которого приготовленное блюдо становится "умирающим".
        /// Используется в плагине штрихкодов.
        /// </summary>
        public static readonly TimeSpan CookedProductDeadDuration = new TimeSpan(0, 2, 0); // 2 minutes

        /// <summary>
        /// Точность, с которой задаётся время приготовления в бэке (1 минута)
        /// </summary>
        public static readonly TimeSpan CookingTimePrecision = new TimeSpan(0, 1, 0); // 1 минута

        /// <summary>
        /// Максимальное количество флаеров в заказе
        /// </summary>
        public const int MaxFlyersPerOrder = 100;

        /// <summary>
        /// Максимальная стоимость заказа, размер платежа.
        /// </summary>
        public const decimal MaxOrderSum = 9999999999999999999m;

        /// <summary>
        /// Номер подачи для непечатаемых позиций заказа (услуг, пополнений).
        /// </summary>
        public const int NonPrintableOrderItemsDefaultServeGroupNumber = 0;

        /// <summary>
        /// Максимальная длина комментария к элементу заказа.
        /// </summary>
        public const int MaxOrderItemCommentLength = 255;

        /// <summary>
        /// Номер курса по умолчанию.
        /// </summary>
        public const int DefaultCourse = 1;

        /// <summary>
        /// Наибольший максимальный курс.
        /// </summary>
        public const int HighestMaxCourse = 20;

        /// <summary>
        /// Максимальный размер внешних данных в строковом представлении для хранения в заказе.
        /// </summary>
        public const int MaxExternalPaymentItemDataLength = 5000;

        /// <summary>
        /// Максимальная длина номера предзаказа.
        /// </summary>
        public const int MaxPreliminaryOrderNumberLength = 50;

        /// <summary>
        /// Максимальная длина купона.
        /// </summary>
        public const int MaxCouponLength = 100;

        /// <summary>
        /// Максимальная длина названия продукта.
        /// </summary>
        public const int MaxProductNameLength = 75;

        /// <summary>
        /// Максимальное количество слотов в одной группе слотов.
        /// </summary>
        public const int MaxSlotsCount = 12;

        /// <summary>
        /// Максимальное количество групп слотов.
        /// </summary>
        public const int MaxSlotGroupsCount = 10;

        /// <summary>
        /// Максимальное количество отображаемых слотов.
        /// </summary>
        public const int MaxVisibleSlotsCount = MaxSlotsCount * MaxSlotGroupsCount;
        #endregion

        #region Banquet, Reserve, Delivery
        /// <summary>
        /// Продолжительность резерва в минутах по умолчанию.
        /// </summary>
        public static readonly TimeSpan DefaultReserveDuration = TimeSpan.FromHours(2);

        /// <summary>
        /// Продолжительность банкета в минутах по умолчанию.
        /// </summary>
        public static readonly TimeSpan DefaultBanquetDuration = TimeSpan.FromHours(3);

        /// <summary>
        /// Минимальная продолжительность резерва.
        /// </summary>
        public static readonly TimeSpan MinReserveDuration = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Максимальная продолжительность резерва.
        /// </summary>
        public static readonly TimeSpan MaxReserveDuration = new TimeSpan(23, 59, 00); // 23h 59m 00s (quick fix of RMS-14576)

        /// <summary>
        /// Минимальная продолжительность банкета.
        /// </summary>
        public static readonly TimeSpan MinBanquetDuration = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Максимальная продолжительность банкета.
        /// </summary>
        public static readonly TimeSpan MaxBanquetDuration = new TimeSpan(23, 59, 00); // 23h 59m 00s (quick fix of RMS-14576)

        /// <summary>
        /// Количество гостей у резерва и банкета по умолчанию.
        /// </summary>
        public const int DefaultReserveBanquetGuestsNum = 2;

        /// <summary>
        /// Время до начала резерва, за которое нужно начать предупреждать о резерве.
        /// </summary>
        public static readonly TimeSpan ReserveWarningTime = TimeSpan.FromMinutes(60);

        /// <summary>
        /// Время до начала банкета, за которое нужно начать предупреждать о банкете.
        /// </summary>
        public static readonly TimeSpan BanquetWarningTime = TimeSpan.FromMinutes(360);

        /// <summary>
        /// Количество резервов, после которого эффективнее обновлять резервы пачкой
        /// </summary>
        public const int ReservesRequiredForBatchUpdate = 4;

        /// <summary>
        /// Максимальная продолжительность доставки, переопределенная на терминале
        /// </summary>
        public static readonly TimeSpan MaxTerminalDeliveryDuration = TimeSpan.FromDays(2);

        /// <summary>
        /// Точность до которой нужно округлить стартовое время банкета, 
        /// в случае если дефолтное время начала банкета прошло
        /// </summary>
        public const int ReservesStartTimeRoundValueAtMinutes = 15;

        public const int CityNameMaxLength = 60;

        public const int StreetNameMaxLength = 60;

        public const int MaxDeliveryRegionNameLength = 25;

        public const int MaxDeliveryProblemCommentLength = 1000;

        public const int MaxAddressAdditionalInfoLength = 500;

        public const int MaxCustomerCommentLength = 500;

        public const int MaxDeliveryCommentLength = 500;

        /// <summary>
        /// Максимальная длина адреса в карточке терминала доставки.
        /// </summary>
        public const int MaxDeliveryTerminalAddressLength = 255;

        /// <summary>
        /// Максимальная длина технической информации в карточке терминала доставки.
        /// </summary>
        public const int MaxDeliveryTerminalTechnicalInformationLength = 1000;

        /// <summary>
        /// Максимальная длина названия источника заказа.
        /// </summary>
        /// <remarks>
        /// При получении заказов из внешних систем плагины интеграции
        /// могут указывать название источника (веб-сайт, мобильный терминал и т.п.),
        /// что позволит строить отчёты в разрезе различных источников и анализировать их эффективность.
        /// </remarks>
        public const int MaxOrderOriginNameLength = 20;

        public const int MaxCustomerNameLength = 60;

        public const int MaxCustomerSurnameLength = 60;

        public const int MaxCustomerNickLength = 60;

        // Эта константа дублируется в проектах плагинов Resto.Front.Api.V8+.
        public const int MaxPhoneLength = 40;

        public const int MaxPhoneCommentLength = 255;

        // Эта константа дублируется в проектах плагинов Resto.Front.Api.V8+.
        public const int MaxEmailLength = 60;

        public const int MaxEmailCommentLength = 255;

        public const int MaxCustomerCardNumberLength = 60;

        public const int MaxDiscountCardNumberLength = 32;

        /// <summary>
        /// Максимальная длина ФИО курьера ВКС.
        /// <remarks>
        /// Примерно равна сумме MaxCustomerNameLength + MaxCustomerSurnameLength + MaxCustomerNameLength для отчества.
        /// </remarks>
        /// </summary>
        public const int MaxEcsCourierFullNameLength = 180;

        /// <summary>
        /// Максимальная длина комментария курьера ВКС.
        /// </summary>
        /// <remarks>
        /// В поле комментария к внешнему курьеру плагин Транспорта обычно передает номер и марку машины
        /// курьера ВКС. Затем эти данные копируются в поле комментария к доставке. Соответственно, длина
        /// комментария к курьеру ВКС не должна превышать максимальную длину комментария доставки.
        /// </remarks>
        public const int MaxEcsCourierCommentLength = 1000;

        /// <summary>
        /// Максимальная длина идентификатора поездки в ВКС.
        /// </summary>
        public const int MaxRideExternalIdLength = 255;

        /// <summary>
        /// Максимальная длина комментария к статусу поездки.
        /// </summary>
        /// <remarks>
        /// Предпологается, что в поле комментария к статусу поездки плагин будет передавать инфо о возникшей ошибке с поездкой.
        /// Эти данные будут отображены в поле комментария к доставке. Соответственно, длина комментария не должна превышать
        /// максимальную длину комментария доставки.
        /// </remarks>
        public const int MaxRideStatusDetailsLength = 1000;

        public const int UaeAddressDetailsMaxLength = 500;

        public const int UaeAddressApartmentMaxLength = 100;

        /// <summary>
        /// Максимальная длина поля Line1. На севрере аналог находится в AddressConstants.java.
        /// </summary>
        public const int AddressLine1MaxLength = 500;

        public const int AddressHouseMaxLength = 10;

        public const int AddressBuildingMaxLength = 10;

        public const int AddressFlatMaxLength = 10;

        public const int AddressEntranceMaxLength = 10;

        public const int AddressFloorMaxLength = 10;

        public const int AddressDoorphoneMaxLength = 10;

        public const int AddressIndexMaxLength = 255;

        public const int AddressExternalCartographyIdMaxLength = 100;

        public static int MaxBlackListReasonLength = 500;

        public static int MarketingSourceMaxLength = 60;

        public static int MaxGuestNameLength = 255;

        public static int MaxCustomerOpinionLength = 1000;

        public static int MaxSurveyItemQuestionLength = 1000;

        public static int MaxComboNameLength = 255;

        public static int MaxActivityTypeLength = 160;

        #endregion

        #region Italy
        public const int ItalyVatAddressCityMaxLength = 60;

        public const int ItalyVatAddressIndexLength = 5;

        public const int ItalyVatAddressStreetMaxLength = 60;

        public const int ItalyVatAddressHouseMaxLength = 8;

        public const int ItalyVatAddressCountyLength = 2;

        public const int ItalyVatAddressRegionLength = 2;

        public const int ItalyIndividualFiscalCodeLength = 11;

        public const int ItalyJurPersonFiscalCodeLength = 16;

        public const int ItalyInnCountryCodeLength = 2;

        public const int ItalyInnCodeMaxLength = 30;

        public const int ItalyItInnCodeMaxLength = 13;

        public const int ItalyRecipientCodeMinLength = 6;

        public const int ItalyRecipientCodeMaxLength = 7;

        public const string ItalyIt = "IT";

        #endregion

        #region France

        public const int FranceNafApeCodeMaxLength = 5;
        public const int FranceSiretCodeMaxLength = 14;

        #endregion

        #region Diagnostics
        /// <summary>
        /// 1 мебибайт в байтах.
        /// </summary>
        public const long MegabyteInBytes = 1 << 20;

        /// <summary>
        /// Порог свободного места на диске при работе фронта, когда выдаётся предупреждение (в мебибайтах).
        /// </summary>
        public const long MinimumFreeSpaceWarningMB = 1024; // 1 Гб

        /// <summary>
        /// Порог свободного места на диске при работе фронта, когда выдаётся критическое предупреждение (в мебибайтах).
        /// </summary>
        public const long MinimumFreeSpaceErrorMB = 100;

        /// <summary>
        /// Интервал времени, по прошествии которого снова повторяются диагностические сообщения (от Watchdog и во фронте),
        /// которые уже были отображены (если ошибки и предупреждения всё ещё имеют место).
        /// </summary>
        public static readonly TimeSpan RepeatDiagnosticMessagesInterval = new TimeSpan(0, 5, 0); // 5 minutes

        /// <summary>
        /// Интервал времени, по прошествии которого будет закрыто предупреждение.
        /// </summary>
        public static readonly TimeSpan RemoveWarningMessagesInterval = new TimeSpan(0, 0, 3); // 3 seconds
        #endregion

        #region Printing
        /// <summary>
        /// Ширина ленты при печати полного отчета.
        /// </summary>
        public const int FullReportTapeWidth = 50;

        /// <summary>
        /// Ширина ленты виртуального принтера.
        /// </summary>
        public const int VirtualPrinterTapeWidth = 40;

        /// <summary>
        /// Ширина ленты при печати в файл.
        /// </summary>
        public const int FilePrinterTapeWidth = 60;

        /// <summary>
        /// Ширина ленты при печати на экран.
        /// </summary>
        public const int ScreenPrinterTapeWidth = 50;

        /// <summary>
        /// Размер шрифта 0 при печати на экран.
        /// </summary>
        public const int ScreenPrinterFontSize0 = 12;

        /// <summary>
        /// Размер шрифта 1 при печати на экран.
        /// </summary>
        public const int ScreenPrinterFontSize1 = 16;

        /// <summary>
        /// Размер шрифта 2 при печати на экран.
        /// </summary>
        public const int ScreenPrinterFontSize2 = 20;

        /// <summary>
        /// Максимальный размер шрифта в pt.
        /// </summary>
        public const int MaxFontSize = 72;

        /// <summary>
        /// Минимальный размер шрифта в pt.
        /// </summary>
        public const int MinFontSize = 1;

        /// <summary>
        /// Вспомогательный тег для шаблона, который не нужно печатать
        /// </summary>
        public const string NoDocTag = "nodoc";
        #endregion

        #region UI
        /// <summary>
        /// Время, в течение которого должен показываться тултип (например, при нажатии на блюдо в заказе)
        /// </summary>
        public static readonly TimeSpan TooltupVisibilityDuration = new TimeSpan(0, 0, 5); //5 секунд
        #endregion

        #region Close cafe session
        /// <summary>
        /// Допустимая погрешность при контрольном пересчёте в рублях.
        /// </summary>
        public const decimal AllowedRecalculateTreshold = 0.1m; // 0 руб 10 коп

        /// <summary>
        /// Максимальная сумма при пересчете при закрытии смены.
        /// </summary>
        public const decimal MaxRecalculateSum = 9999999999999999999m;

        public const decimal MaxProductPrice = 9999999999999999999m;
        #endregion

        #region Cash register
        /// <summary>
        /// Идентификатор регистра ФР по умолчанию.
        /// </summary>
        public const string DefaultPaymentRegisterId = "";

        /// <summary>
        /// Формат даты для ФР.
        /// </summary>
        public const string CashRegisterDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        #endregion

        #region Correction Cheque
        public static int MaxCorrectionReasonLength = 100;
        public static int MaxDocumentNumberLength = 100;
        #endregion

        #region Journal events
        public const int MaxJournalEventSenderLength = 255;
        public const int MaxJournalEventEventTypeLength = 255;
        public const int MaxJournalEventAttributeNameLength = 255;
        public const int MaxJournalEventAttributeValueLength = 1000;
        #endregion

        #region Devices

        public const int DeviceNameLength = 100;

        public static readonly int[] DeviceBaudRates = { 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200 };

        public const int MaxScalesDescriptionWidth = 512;
        public const int MinScalesDescriptionWidth = 1;
        public const int MaxScalesLocalUpd = 65535;
        public const int MinScalesLocalUpd = 1;
        public const int MaxScalesPortNumber = 255;
        public const int MinScalesPortNumber = 1;
        public const int MaxScalesPortTimeout = 60000;
        public const int MinScalesPortTimeout = 1;
        public const int MaxScalesRemoteTcp = 65535;
        public const int MinScalesRemoteTcp = 1;
        public const int MaxScalesRemoteUpd = 65535;
        public const int MinScalesRemoteUpd = 1;
        public const int MaxScalesTimeStable = 60000;
        public const int MinScalesTimeStable = 1;

        public static readonly int[] PrinterDpi = { 203, 300 };
        public const int MaxPrinterMarginHorizontal = 2300;
        public const int MinPrinterMarginHorizontal = 1;
        public const int MaxPrinterLabelHeight = 2300;
        public const int MinPrinterLabelHeight = 100;
        public const int MaxPrinterLabelWidth = 2300;
        public const int MinPrinterLabelWidth = 200;
        public const int MaxPrinterLinesCount = 100;
        public const int MinPrinterLinesCount = 1;
        public const int MaxTcpipPrinterPort = 65535;
        public const int MaxComPrinterPort = 255;
        public const int MinPrinterPortNumber = 1;
        public const int MaxPrinterPulsePort = 65535;
        public const int MinPrinterPulsePort = 0;
        public const int MaxPrinterMarginVertical = 2300;
        public const int MinPrinterMarginVertical = 0;
        public const int MaxPrinterSpoolerPingInterval = 9999;
        public const int MinPrinterSpoolerPingInterval = 1;
        public const int MaxPrinterSpoolerTaskExpiryDuration = 9999;
        public const int MinPrinterSpoolerTaskExpiryDuration = 0;

        public const int MaxCashRegisterPortNumber = 255;
        public const int MinCashRegisterPortNumber = 1;
        public const int MaxCashRegisterChequeTextAttributes = 999;
        public const int MinCashRegisterChequeTextAttributes = 1;
        public const int MaxCashRegisterChequeTextChars = 999;
        public const int MinCashRegisterChequeTextChars = 1;
        public const int MaxCashRegisterCustomerDisplayChars = 999;
        public const int MinCashRegisterCustomerDisplayChars = 1;
        public const int MaxCashRegisterCustomerDisplayRows = 999;
        public const int MinCashRegisterCustomerDisplayRows = 1;

        #endregion

        #region DataMatrixMark
        /// <summary>
        /// Длина gtin(код товара) DataMatrix марки.
        /// </summary>
        public const int LengthOfGtinMark = 14;
        /// <summary>
        /// Максимальная длина табачной марки.
        /// </summary>
        public const int MaxMarkLength = 255;
        /// <summary>
        /// Минимальная длина табачной марки.
        /// </summary>
        public const int MinLengthOfTobaccoMark = 21;
        /// <summary>
        /// Длина серии табачной марки.
        /// </summary>
        public const int LengthOfSerialTobaccoMark = 7;
        /// <summary>
        /// Длина серии молочной продукции.
        /// </summary>
        public const int LengthOfSerialDairyProduct = 6;
        /// <summary>
        /// Длина серии молочной продукции с весом.
        /// </summary>
        public const int LengthOfSerialWeightedDairyProduct = 6;
        /// <summary>
        /// Длина серии упакованной воды.
        /// </summary>
        public const int LengthOfSerialBottledWater = 13;
        /// <summary>
        /// Длина информации о цене табачной марки.
        /// </summary>
        public const int LengthOfPriceTobaccoMark = 6;
        /// <summary>
        /// Длина информации о весе молочной продукции с весом.
        /// </summary>
        public const int LengthOfWeightDairyProduct = 6;
        /// <summary>
        /// Длина дополнительной информации о табачной марке(цена и серия).
        /// </summary>
        public const int LengthOfAdditionalPartTobaccoMark = 11;
        /// <summary>
        /// Код маркировки табачной продукции.
        /// </summary>
        public const string CodeOfTobaccoMark = "444D";
        /// <summary>
        /// Идентификатор для цены единицы табачной продукции .
        /// </summary>
        public const string CodeOfPriceTobaccoMark = "8005";
        /// <summary>
        /// Идентификатор для номера марки.
        /// </summary>
        public const string CodeOfGtinMark = "01";
        /// <summary>
        /// Идентификатор для серии марки.
        /// </summary>
        public const string CodeOfSerialTobaccoMark = "21";
        /// <summary>
        /// Разделитель информации в марке в формате GS1.
        /// </summary>
        public const string SeparatorMark = "\u001d";
        /// <summary>
        /// Резрешенное количество товара с маркировкой.
        /// </summary>
        public const decimal AllowedItemAmountWithMark = 1m;
        /// <summary>
        /// Длина кода маркировки для табачной продукции (пачка).
        /// </summary>
        public const int TobaccoMarkLength = 29;
        /// <summary>
        /// Длина кода маркировки для табачной продукции (блок).
        /// </summary>
        public const int TobaccoBlockMarkLength = 43;
        /// <summary>
        /// Длина кода маркировки для молочных продуктов.
        /// </summary>
        public const int DairyProductMarkLength = 31;
        /// <summary>
        /// Длина кода маркировки для молочных продуктов c весом.
        /// </summary>
        public const int WeightedDairyProductMarkLength = 42;
        /// <summary>
        /// Длина кода маркировки для молочных продуктов c весом.
        /// </summary>
        public const int BottledWaterMarkLength = 38;
        #endregion

        #region Egais
        /// <summary>
        /// Минимальный объем нефасованного алкоголя в литрах.
        /// </summary>
        public const decimal MinimumEgaisUnpackedAmount = 0.01m;
        /// <summary>
        /// Длинна алкокода.
        /// </summary>
        public const int AlcCodeLength = 19;
        public const char AlcCodeFillChar = '0';
        #endregion

        #region Custom data

        // При изменении констант блока Custom data убедиться, что эти константы влезают в ограничения WCF-сервиса CustomDataService (serviceModel.Bindings.config).

        public const int MaxCustomDataKeyLength = 512;

        public const int MaxCustomDataValueLength = 32768;

        public const int MaxCustomDataItemsCountPerPlugin = 1024;

        #endregion
    }
}
