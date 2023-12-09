// This file was generated with XmlToCodeGenerator.
// Do not edit it manually.


namespace Resto.Front.Localization.Resources
{
    public static partial class IikoCard51LocalResources
    {
        private static readonly Resto.Framework.Localization.Localizer Localizer = Resto.Framework.Localization.Localizer.Create("IikoCard51LocalResources.resx", "Resto.Front.Localization.Resources.IikoCard51LocalResources", System.Reflection.Assembly.GetExecutingAssembly());

        /// <summary>
        ///   Looks up a localized string similar to "Авторизация"
        /// </summary>
        public static System.String AuthTitle
        {
            get { return Localizer.GetStringFromResources("AuthTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Введите номер"
        /// </summary>
        public static System.String AuthCardHint
        {
            get { return Localizer.GetStringFromResources("AuthCardHint"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Или сделайте фото"
        /// </summary>
        public static System.String AuthByPhotoHint
        {
            get { return Localizer.GetStringFromResources("AuthByPhotoHint"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "POS-сервер недоступен."
        /// </summary>
        public static System.String IikoCard5PosServerIsNotAvailable
        {
            get { return Localizer.GetStringFromResources("IikoCard5PosServerIsNotAvailable"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "POS-сервер в режиме онлайн."
        /// </summary>
        public static System.String IikoCard5PosServerIsOnline
        {
            get { return Localizer.GetStringFromResources("IikoCard5PosServerIsOnline"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "POS-сервер в автономном режиме. Овердрафт доступен. Последний обмен: {0}.{1}"
        /// </summary>
        public static System.String IikoCard5PosServerIsOfflineFormat
        {
            get { return Localizer.GetStringFromResources("IikoCard5PosServerIsOfflineFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "POS-сервер в автономном режиме. Овердрафт недоступен. Последний обмен: {0}.{1}"
        /// </summary>
        public static System.String IikoCard5PosServerIsOfflineNotSupportedFormat
        {
            get { return Localizer.GetStringFromResources("IikoCard5PosServerIsOfflineNotSupportedFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "${LOYALTY_APP}: Привязать карту к телефону"
        /// </summary>
        public static System.String BindCardToPhoneButtonTitle
        {
            get { return Localizer.GetStringFromResources("BindCardToPhoneButtonTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "${LOYALTY_APP}: Запрос баланса"
        /// </summary>
        public static System.String ShowWalletsBalancesButtonTitle
        {
            get { return Localizer.GetStringFromResources("ShowWalletsBalancesButtonTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "${LOYALTY_APP}: Принудительная синхронизация"
        /// </summary>
        public static System.String ForceSynchronizeButtonTitle
        {
            get { return Localizer.GetStringFromResources("ForceSynchronizeButtonTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "${LOYALTY_APP}: Диагностика подключения к {0}"
        /// </summary>
        public static System.String DiagnosticsButtonTitle
        {
            get { return Localizer.GetStringFromResources("DiagnosticsButtonTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "${LOYALTY_APP}: Активация сертификата"
        /// </summary>
        public static System.String CertificateActivationTitle
        {
            get { return Localizer.GetStringFromResources("CertificateActivationTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Карта не найдена."
        /// </summary>
        public static System.String FaultReasonCardNotFound
        {
            get { return Localizer.GetStringFromResources("FaultReasonCardNotFound"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Баланс счета гостя недостаточен для оплаты данного заказа."
        /// </summary>
        public static System.String FaultReasonInsufficientFunds
        {
            get { return Localizer.GetStringFromResources("FaultReasonInsufficientFunds"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Ошибка соединения с POS-сервером."
        /// </summary>
        public static System.String ConnectionFailedError
        {
            get { return Localizer.GetStringFromResources("ConnectionFailedError"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Превышено время ожидания запроса к POS-серверу."
        /// </summary>
        public static System.String ServiceTimeoutError
        {
            get { return Localizer.GetStringFromResources("ServiceTimeoutError"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Версия протокола не поддерживается на POS-сервере. Обмен данными с POS-сервером отключен."
        /// </summary>
        public static System.String WrongProtocolVersion
        {
            get { return Localizer.GetStringFromResources("WrongProtocolVersion"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Внимание, POS-сервер уже привязан к другой организации. Если Вы хотите привязать POS-сервер к заданной организации необходимо зайти на ${CLOUD_API_LEGACY} в раздел Администрирование -> Мониторинг POS-серверов и выполнить удаление данных POS-сервера"
        /// </summary>
        public static System.String PosBindedToAnotherOrganization
        {
            get { return Localizer.GetStringFromResources("PosBindedToAnotherOrganization"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Обмен данными с POS-сервером отключен."
        /// </summary>
        public static System.String DataExchangeWithPosDisabled
        {
            get { return Localizer.GetStringFromResources("DataExchangeWithPosDisabled"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Сумма оплаты {0} меньше минимально допустимой ({1})."
        /// </summary>
        public static System.String PayOperationFailedSumIsLessThenMinLimit
        {
            get { return Localizer.GetStringFromResources("PayOperationFailedSumIsLessThenMinLimit"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Сумма оплаты {0} больше максимально допустимой ({1})."
        /// </summary>
        public static System.String PayOperationFailedSumIsGreaterThenMaxLimit
        {
            get { return Localizer.GetStringFromResources("PayOperationFailedSumIsGreaterThenMaxLimit"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Оплата"
        /// </summary>
        public static System.String PaymentViewTitle
        {
            get { return Localizer.GetStringFromResources("PaymentViewTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Можно оплатить до {0}"
        /// </summary>
        public static System.String MaxPaymentSumFormat
        {
            get { return Localizer.GetStringFromResources("MaxPaymentSumFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "({0} бон.)"
        /// </summary>
        public static System.String BonusesSumShortFormat
        {
            get { return Localizer.GetStringFromResources("BonusesSumShortFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Для данного гостя и состава заказа максимальная сумма оплаты равна нулю."
        /// </summary>
        public static System.String MaxPaymentSumIsZero
        {
            get { return Localizer.GetStringFromResources("MaxPaymentSumIsZero"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Гость не включен ни в одну программу."
        /// </summary>
        public static System.String GuestDoesNotBelongToAnyProgram
        {
            get { return Localizer.GetStringFromResources("GuestDoesNotBelongToAnyProgram"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Невозможно распределить сумму оплаты по кошелькам с указанными лимитами. Пожалуйста, отредактируйте оплату '{0}', чтобы установить новые лимиты."
        /// </summary>
        public static System.String PayOperationFailedCanNotSpreadPaymentWithGivenLimits
        {
            get { return Localizer.GetStringFromResources("PayOperationFailedCanNotSpreadPaymentWithGivenLimits"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Номер карты"
        /// </summary>
        public static System.String CardNumberHint
        {
            get { return Localizer.GetStringFromResources("CardNumberHint"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Введите номер телефона"
        /// </summary>
        public static System.String BindCardEnterPhone
        {
            get { return Localizer.GetStringFromResources("BindCardEnterPhone"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Номер телефона"
        /// </summary>
        public static System.String BindCardEnterPhoneHint
        {
            get { return Localizer.GetStringFromResources("BindCardEnterPhoneHint"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Карта успешно привязана к номеру телефона."
        /// </summary>
        public static System.String BindCardCompletedSuccessfully
        {
            get { return Localizer.GetStringFromResources("BindCardCompletedSuccessfully"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Ошибка при привязке карты.{0}{1}"
        /// </summary>
        public static System.String BindCardError
        {
            get { return Localizer.GetStringFromResources("BindCardError"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Привязка карты к телефону..."
        /// </summary>
        public static System.String BindCardToPhoneInProgress
        {
            get { return Localizer.GetStringFromResources("BindCardToPhoneInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Получение информации о кошельках гостя..."
        /// </summary>
        public static System.String GetWalletsBalancesInProgress
        {
            get { return Localizer.GetStringFromResources("GetWalletsBalancesInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Получение информации о состоянии поса..."
        /// </summary>
        public static System.String GetPosStateInProgress
        {
            get { return Localizer.GetStringFromResources("GetPosStateInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Запрос принудительной синхронизации..."
        /// </summary>
        public static System.String ForceSyncRequestInProgress
        {
            get { return Localizer.GetStringFromResources("ForceSyncRequestInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Регистрация гостя..."
        /// </summary>
        public static System.String RegisterGuestInProgress
        {
            get { return Localizer.GetStringFromResources("RegisterGuestInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Обновление заказа..."
        /// </summary>
        public static System.String UpdateOrderInProgress
        {
            get { return Localizer.GetStringFromResources("UpdateOrderInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Получение информации о рекомендателе..."
        /// </summary>
        public static System.String GetCustomerReferrerInfoInProgress
        {
            get { return Localizer.GetStringFromResources("GetCustomerReferrerInfoInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Фиксация скидок..."
        /// </summary>
        public static System.String FixDiscounts
        {
            get { return Localizer.GetStringFromResources("FixDiscounts"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Обновление стоимости комбо..."
        /// </summary>
        public static System.String GetComboPriceInProgress
        {
            get { return Localizer.GetStringFromResources("GetComboPriceInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Привязка сертификата..."
        /// </summary>
        public static System.String BindCertificateInProgress
        {
            get { return Localizer.GetStringFromResources("BindCertificateInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Дополнительные операции недоступны, так как нет лицензии на ${LOYALTY_APP}."
        /// </summary>
        public static System.String AdditionalOperationsNotAvailable
        {
            get { return Localizer.GetStringFromResources("AdditionalOperationsNotAvailable"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Кошельки гостя {0}"
        /// </summary>
        public static System.String UserWalletsHintFormat
        {
            get { return Localizer.GetStringFromResources("UserWalletsHintFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Время обмена: {0}"
        /// </summary>
        public static System.String DiagnosticsLastExchangeHintFormat
        {
            get { return Localizer.GetStringFromResources("DiagnosticsLastExchangeHintFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Кошелек"
        /// </summary>
        public static System.String UserWalletNameHeaderTitle
        {
            get { return Localizer.GetStringFromResources("UserWalletNameHeaderTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Баланс"
        /// </summary>
        public static System.String UserWalletBalanceHeaderTitle
        {
            get { return Localizer.GetStringFromResources("UserWalletBalanceHeaderTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "неизвестно"
        /// </summary>
        public static System.String LastExchangedDateUnknown
        {
            get { return Localizer.GetStringFromResources("LastExchangedDateUnknown"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Диагностика подключения ${LOYALTY_APP}"
        /// </summary>
        public static System.String DiagnosticsTitle
        {
            get { return Localizer.GetStringFromResources("DiagnosticsTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Название"
        /// </summary>
        public static System.String DiagnosticsNameHeaderTitle
        {
            get { return Localizer.GetStringFromResources("DiagnosticsNameHeaderTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Состояние"
        /// </summary>
        public static System.String DiagnosticsValueHeaderTitle
        {
            get { return Localizer.GetStringFromResources("DiagnosticsValueHeaderTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Информация"
        /// </summary>
        public static System.String DiagnosticsInfoHeaderTitle
        {
            get { return Localizer.GetStringFromResources("DiagnosticsInfoHeaderTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Доступен"
        /// </summary>
        public static System.String DiagnosticsPosAvailable
        {
            get { return Localizer.GetStringFromResources("DiagnosticsPosAvailable"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Обмен был меньше 10 минут назад"
        /// </summary>
        public static System.String DiagnosticsPosOnline
        {
            get { return Localizer.GetStringFromResources("DiagnosticsPosOnline"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Доступен RabbitMQ"
        /// </summary>
        public static System.String DiagnosticsRabbitMQAvailable
        {
            get { return Localizer.GetStringFromResources("DiagnosticsRabbitMQAvailable"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Установлена последняя версия"
        /// </summary>
        public static System.String DiagnosticsIsLastVersion
        {
            get { return Localizer.GetStringFromResources("DiagnosticsIsLastVersion"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Да"
        /// </summary>
        public static System.String DiagnosticsSuccess
        {
            get { return Localizer.GetStringFromResources("DiagnosticsSuccess"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Нет"
        /// </summary>
        public static System.String DiagnosticsFail
        {
            get { return Localizer.GetStringFromResources("DiagnosticsFail"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Принудительная синхронизация запущена."
        /// </summary>
        public static System.String ForceSyncStartedSuccessfully
        {
            get { return Localizer.GetStringFromResources("ForceSyncStartedSuccessfully"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось запустить принудительную синхронизацию."
        /// </summary>
        public static System.String ForceSyncError
        {
            get { return Localizer.GetStringFromResources("ForceSyncError"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to " Ошибка обработки в ${CLOUD_API_LEGACY}."
        /// </summary>
        public static System.String ExchangeWithIikoBizError
        {
            get { return Localizer.GetStringFromResources("ExchangeWithIikoBizError"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось закрыть заказ на POS-сервере."
        /// </summary>
        public static System.String CloseOperationFailed
        {
            get { return Localizer.GetStringFromResources("CloseOperationFailed"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось вернуть заказ на POS-сервере."
        /// </summary>
        public static System.String RefundOperationFailed
        {
            get { return Localizer.GetStringFromResources("RefundOperationFailed"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось отменить заказ на POS-сервере."
        /// </summary>
        public static System.String ResetOperationFailed
        {
            get { return Localizer.GetStringFromResources("ResetOperationFailed"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось отменить заказ на POS-сервере."
        /// </summary>
        public static System.String AbortOperationFailed
        {
            get { return Localizer.GetStringFromResources("AbortOperationFailed"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Купон"
        /// </summary>
        public static System.String CouponTitle
        {
            get { return Localizer.GetStringFromResources("CouponTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Рекомендатель"
        /// </summary>
        public static System.String CustomerReferrerTitle
        {
            get { return Localizer.GetStringFromResources("CustomerReferrerTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Купон '{0}' более не действителен или не выполнены все условия акции."
        /// </summary>
        public static System.String CouponInvalidOrUnappliedFormat
        {
            get { return Localizer.GetStringFromResources("CouponInvalidOrUnappliedFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Ошибка при обращении к POS-серверу."
        /// </summary>
        public static System.String CouponApplyingError
        {
            get { return Localizer.GetStringFromResources("CouponApplyingError"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось присвоить рекомендателя гостю."
        /// </summary>
        public static System.String SetCustomerReferrerFailed
        {
            get { return Localizer.GetStringFromResources("SetCustomerReferrerFailed"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Рекомендатель с номером телефона {0} не найден. Попробуйте ввести другой номер."
        /// </summary>
        public static System.String CustomerReferrerWasNotFoundByPhone
        {
            get { return Localizer.GetStringFromResources("CustomerReferrerWasNotFoundByPhone"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Рекомендатель с e-mail {0} не найден. Попробуйте ввести другой e-mail."
        /// </summary>
        public static System.String CustomerReferrerWasNotFoundByEmail
        {
            get { return Localizer.GetStringFromResources("CustomerReferrerWasNotFoundByEmail"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Рекомендатель {0} успешно присвоен гостю."
        /// </summary>
        public static System.String CustomerReferrerHasBeenSet
        {
            get { return Localizer.GetStringFromResources("CustomerReferrerHasBeenSet"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Выбрать другое"
        /// </summary>
        public static System.String ChooseAnotherComboGroupProduct
        {
            get { return Localizer.GetStringFromResources("ChooseAnotherComboGroupProduct"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Комбо '{0}' более не активно для редактируемого заказа."
        /// </summary>
        public static System.String ComboNotExistsOnIikoCardFormat
        {
            get { return Localizer.GetStringFromResources("ComboNotExistsOnIikoCardFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Спецификация комбо '{0}' была изменена. Комбо более не доступно для редактирования."
        /// </summary>
        public static System.String ComboSpecificationNotValidFormat
        {
            get { return Localizer.GetStringFromResources("ComboSpecificationNotValidFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Невозможно собрать '{0}'."
        /// </summary>
        public static System.String CanNotCollectComboFormat
        {
            get { return Localizer.GetStringFromResources("CanNotCollectComboFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Комбо"
        /// </summary>
        public static System.String ComboDynamicRegionButtonTitle
        {
            get { return Localizer.GetStringFromResources("ComboDynamicRegionButtonTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Соберите '{0}'"
        /// </summary>
        public static System.String BuildComboFormat
        {
            get { return Localizer.GetStringFromResources("BuildComboFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "ПОКАЗАТЬ СОБРАННОЕ КОМБО"
        /// </summary>
        public static System.String ComboSummarizeStepTitle
        {
            get { return Localizer.GetStringFromResources("ComboSummarizeStepTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Предложите добавить блюдо и соберите комбо '{0}'"
        /// </summary>
        public static System.String OfferProductAndBuildComboFormat
        {
            get { return Localizer.GetStringFromResources("OfferProductAndBuildComboFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "ДОБАВЬТЕ ПОДАРОК"
        /// </summary>
        public static System.String FreeProductGroupName
        {
            get { return Localizer.GetStringFromResources("FreeProductGroupName"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "ПРЕДЛОЖИТЕ ГОСТЮ"
        /// </summary>
        public static System.String UpsaleGroupName
        {
            get { return Localizer.GetStringFromResources("UpsaleGroupName"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "СОБЕРИТЕ КОМБО-БЛЮДО"
        /// </summary>
        public static System.String ComboGroupName
        {
            get { return Localizer.GetStringFromResources("ComboGroupName"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "ДОБАВЬТЕ БЛЮДО ДЛЯ КОМБО"
        /// </summary>
        public static System.String AddProductAndCollectComboGroupName
        {
            get { return Localizer.GetStringFromResources("AddProductAndCollectComboGroupName"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "ИНФОРМАЦИЯ"
        /// </summary>
        public static System.String InformationGroupName
        {
            get { return Localizer.GetStringFromResources("InformationGroupName"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Акция: {0}"
        /// </summary>
        public static System.String ActionNameFormat
        {
            get { return Localizer.GetStringFromResources("ActionNameFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось применить условие ручного подтверждения. Пожалуйста, повторите попытку."
        /// </summary>
        public static System.String ManualConditionApplyError
        {
            get { return Localizer.GetStringFromResources("ManualConditionApplyError"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Лицензионное ограничение: оплата ${LOYALTY_APP} недоступна."
        /// </summary>
        public static System.String IikoCard51IsNotActiveForOrderBecauseOfLicense
        {
            get { return Localizer.GetStringFromResources("IikoCard51IsNotActiveForOrderBecauseOfLicense"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "${LOYALTY_APP} не настроен для работы в текущем ресторане."
        /// </summary>
        public static System.String IikoCard51SettingsNotSpecifiedForCurrentDepartment
        {
            get { return Localizer.GetStringFromResources("IikoCard51SettingsNotSpecifiedForCurrentDepartment"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "${LOYALTY_APP} не настроен для работы в текущем ресторане. Обратитесь к администратору системы для настройки ${LOYALTY_APP} и выполните обмен данными с ЦО."
        /// </summary>
        public static System.String IikoCard51SettingsNotExists
        {
            get { return Localizer.GetStringFromResources("IikoCard51SettingsNotExists"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "{0:N0} шт."
        /// </summary>
        public static System.String AmountShortFormat
        {
            get { return Localizer.GetStringFromResources("AmountShortFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Подсказок нет"
        /// </summary>
        public static System.String NoPromts
        {
            get { return Localizer.GetStringFromResources("NoPromts"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Доступных для продажи комбо-блюд нет"
        /// </summary>
        public static System.String NoAvailableCombos
        {
            get { return Localizer.GetStringFromResources("NoAvailableCombos"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "ПРОМО-КОД: {0}"
        /// </summary>
        public static System.String MobileAuthorizationCodeFormat
        {
            get { return Localizer.GetStringFromResources("MobileAuthorizationCodeFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "В заказ уже добавлена оплата на максимально возможную сумму."
        /// </summary>
        public static System.String OrderContainsFullOrderPayments
        {
            get { return Localizer.GetStringFromResources("OrderContainsFullOrderPayments"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Активация сертификата"
        /// </summary>
        public static System.String ActivateCertificateTitle
        {
            get { return Localizer.GetStringFromResources("ActivateCertificateTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Введите номер сертификата"
        /// </summary>
        public static System.String SetCertificateNumberTitle
        {
            get { return Localizer.GetStringFromResources("SetCertificateNumberTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Привязка сертификата"
        /// </summary>
        public static System.String BindCertificateTitle
        {
            get { return Localizer.GetStringFromResources("BindCertificateTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Активировать"
        /// </summary>
        public static System.String ActivateCertificateButton
        {
            get { return Localizer.GetStringFromResources("ActivateCertificateButton"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Указать гостя"
        /// </summary>
        public static System.String SetGuestButton
        {
            get { return Localizer.GetStringFromResources("SetGuestButton"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Сертификат успешно активирован"
        /// </summary>
        public static System.String CertificateActivatedSuccessfully
        {
            get { return Localizer.GetStringFromResources("CertificateActivatedSuccessfully"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Активация сертификата..."
        /// </summary>
        public static System.String CertificateActivationProgress
        {
            get { return Localizer.GetStringFromResources("CertificateActivationProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Сертификату будет привязан гость '{0}'"
        /// </summary>
        public static System.String CertificateWillBeBindedToGuestFormat
        {
            get { return Localizer.GetStringFromResources("CertificateWillBeBindedToGuestFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Гость с номером карты '{0}' не найден"
        /// </summary>
        public static System.String GuestByCardNumberNotFoundFormat
        {
            get { return Localizer.GetStringFromResources("GuestByCardNumberNotFoundFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Гость с трэком '{0}' не найден"
        /// </summary>
        public static System.String GuestByTrackNotFoundFormat
        {
            get { return Localizer.GetStringFromResources("GuestByTrackNotFoundFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Тип оплаты '{0}' настроен не правильно."
        /// </summary>
        public static System.String IncorrectPaymentTypeFormat
        {
            get { return Localizer.GetStringFromResources("IncorrectPaymentTypeFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Верификация"
        /// </summary>
        public static System.String VerifyGuestTitle
        {
            get { return Localizer.GetStringFromResources("VerifyGuestTitle"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Введите код подтверждения"
        /// </summary>
        public static System.String SetVerificationCode
        {
            get { return Localizer.GetStringFromResources("SetVerificationCode"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Отправить снова"
        /// </summary>
        public static System.String SendVerificationCode
        {
            get { return Localizer.GetStringFromResources("SendVerificationCode"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не получилось отправить SMS с кодом подтверждения. Ошибка на сервере."
        /// </summary>
        public static System.String CanNotSendGuestVerificationSmsBecauseOfRmsError
        {
            get { return Localizer.GetStringFromResources("CanNotSendGuestVerificationSmsBecauseOfRmsError"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не получилось отправить SMS с кодом подтверждения. Сервер недоступен."
        /// </summary>
        public static System.String CanNotSendGuestVerificationSmsBecauseRmsIsNotAvailable
        {
            get { return Localizer.GetStringFromResources("CanNotSendGuestVerificationSmsBecauseRmsIsNotAvailable"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Отправка кода верификации."
        /// </summary>
        public static System.String GuestVerificationCodeSending
        {
            get { return Localizer.GetStringFromResources("GuestVerificationCodeSending"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Код введен неверно. Попробуйте ввести код еще раз."
        /// </summary>
        public static System.String IncorrectCodeTryAgain
        {
            get { return Localizer.GetStringFromResources("IncorrectCodeTryAgain"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Превышено кол-во попыток отправки кода."
        /// </summary>
        public static System.String NumberOfSentVerificationMesagesExceeded
        {
            get { return Localizer.GetStringFromResources("NumberOfSentVerificationMesagesExceeded"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Код введен неверно. Превышено кол-во попыток ввода текущего кода."
        /// </summary>
        public static System.String NumberOfAttemptsToEnterTheCurrentVerirficationCodeExceeded
        {
            get { return Localizer.GetStringFromResources("NumberOfAttemptsToEnterTheCurrentVerirficationCodeExceeded"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "{0} - Ваш код подтверждения."
        /// </summary>
        public static System.String VerificationGuestMessage
        {
            get { return Localizer.GetStringFromResources("VerificationGuestMessage"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не получилось отправить SMS с кодом подтверждения. <br />
        /// У Вашей организации недостаточный баланс SMS."
        /// </summary>
        public static System.String CanNotSendGuestVerificationSmsBecauseOfZeroSmsBalance
        {
            get { return Localizer.GetStringFromResources("CanNotSendGuestVerificationSmsBecauseOfZeroSmsBalance"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Обмен данными с POS включен"
        /// </summary>
        public static System.String DiagnosticsDataExchangeWithPosEnabled
        {
            get { return Localizer.GetStringFromResources("DiagnosticsDataExchangeWithPosEnabled"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Сертификат успешно активирован и добавлен гостю"
        /// </summary>
        public static System.String CertificateActivatedAndBindedToGuestSuccessfully
        {
            get { return Localizer.GetStringFromResources("CertificateActivatedAndBindedToGuestSuccessfully"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Автоматическая сборка комбо не выполнена: {0}"
        /// </summary>
        public static System.String GetOptimalComboErrorFormat
        {
            get { return Localizer.GetStringFromResources("GetOptimalComboErrorFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Информация по подарочному сертификату"
        /// </summary>
        public static System.String CertificateInfoHintFormat
        {
            get { return Localizer.GetStringFromResources("CertificateInfoHintFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Получение информации по подарочному сертификату..."
        /// </summary>
        public static System.String GetCertificateInfoInProgress
        {
            get { return Localizer.GetStringFromResources("GetCertificateInfoInProgress"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось найти сертификат '{0}'."
        /// </summary>
        public static System.String CertificateNotFoundWarning
        {
            get { return Localizer.GetStringFromResources("CertificateNotFoundWarning"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Подарочный сертификат"
        /// </summary>
        public static System.String Certificate
        {
            get { return Localizer.GetStringFromResources("Certificate"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Выберите тип"
        /// </summary>
        public static System.String ChooseWalletType
        {
            get { return Localizer.GetStringFromResources("ChooseWalletType"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Баланс гостя"
        /// </summary>
        public static System.String GuestBalance
        {
            get { return Localizer.GetStringFromResources("GuestBalance"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Сертификат заблокирован. Причина: {0}."
        /// </summary>
        public static System.String CertificateIsBlocked
        {
            get { return Localizer.GetStringFromResources("CertificateIsBlocked"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Сертификат удален."
        /// </summary>
        public static System.String CertificateIsDeleted
        {
            get { return Localizer.GetStringFromResources("CertificateIsDeleted"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Срок действия сертификата истек."
        /// </summary>
        public static System.String CertificateIsExpired
        {
            get { return Localizer.GetStringFromResources("CertificateIsExpired"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Активирован"
        /// </summary>
        public static System.String IsActivе
        {
            get { return Localizer.GetStringFromResources("IsActivе"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Указан неправильный купон '{0}'."
        /// </summary>
        public static System.String CouponInvalidFormat
        {
            get { return Localizer.GetStringFromResources("CouponInvalidFormat"); }
        }

        /// <summary>
        ///   Looks up a localized string similar to "Не удалось применить купон '{0}', не выполнены все условия акции."
        /// </summary>
        public static System.String CouponUnappliedFormat
        {
            get { return Localizer.GetStringFromResources("CouponUnappliedFormat"); }
        }

    }
}

