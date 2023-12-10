using System;
using System.Collections.Generic;
using System.ServiceModel;
using Resto.Framework.Attributes.JetBrains;
using Resto.Front.Localization.Resources;

namespace Resto.Common.Plastek
{
    /// <summary>
    /// Класс-обертка над вызовами SOAP методов сервиса Плас-Тек.
    /// <remarks>
    /// В app.config приложения, которое будет использовать этот класс, нужно
    /// добавить binding и endpoint Плас-Тек, которые находятся в файле Plastek.config.
    /// По умолчанию в этом файле определен binding, поддеживающий работу по SSL.
    /// В рантайме смена протокола не поддерживается, поэтому, если пользователь в настройках в бек-оффисе
    /// задаст адрес "http" вместо "https", то он получит ошибку о том, что схема не соответствует ожидаемой.
    /// Поскольку Плас-Тек в первую очередь работает по https (http-соединения являются либо тестовыми, 
    /// либо включаются по запросам конкретных клиентов - при этом https все равно поддерживается), то невозможность
    /// в рантайме сменить протокол является некритичной (и возможно, даже это полезно для целей секьюрности).
    /// Если все же нужна смена "https" на "http", то нужно изменить binding.
    /// Подробности о том, как это сделать см. в файле Plastek.config.
    /// </remarks>
    /// </summary>
    public static class PlastekService
    {
        private static Dictionary<PlastekServiceError, string> errorCodesToDescriptions;

        //Строим свойство лениво, это гарантирует, что данные о текущей локали уже загружены.
        //Смена локали на лету не поддерживается
        private static Dictionary<PlastekServiceError, string> ErrorCodesToDescriptions
        {
            get { return errorCodesToDescriptions ?? (errorCodesToDescriptions = BuildDescriptions()); }
        }

        private static Dictionary<PlastekServiceError, string> BuildDescriptions()
        {
            return new Dictionary<PlastekServiceError, string>
                { 
                    { PlastekServiceError.Uninitialized, PlastekResources.UninitializedDescription },
                    { PlastekServiceError.AuthenticationError, PlastekResources.AuthenticationErrorDescription },
                    { PlastekServiceError.AuthorizationError, PlastekResources.AuthorizationErrorDescription },
                    { PlastekServiceError.GeneralError, PlastekResources.GeneralErrorDescription },
                    { PlastekServiceError.DatabaseError, PlastekResources.DatabaseErrorDescription },
                    { PlastekServiceError.DatabaseConnectError, PlastekResources.DatabaseConnectErrorDescription },
                    { PlastekServiceError.ItemExists, PlastekResources.ItemExistsDescription },
                    { PlastekServiceError.InvalidCard, PlastekResources.InvalidCardDescription },
                    { PlastekServiceError.InvalidPin, PlastekResources.InvalidPinDescription },
                    { PlastekServiceError.ExpiredCard, PlastekResources.ExpiredCardDescription },
                    { PlastekServiceError.ActiveCard, PlastekResources.ActiveCardDescription },
                    { PlastekServiceError.InactiveCard, PlastekResources.InactiveCardDescription },
                    { PlastekServiceError.StoppedCard, PlastekResources.StoppedCardDescription },
                    { PlastekServiceError.CanceledCard, PlastekResources.CanceledCardDescription },
                    { PlastekServiceError.InsufficientBalance, PlastekResources.InsufficientBalanceDescription },
                    { PlastekServiceError.InvalidMerchant, PlastekResources.InvalidMerchantDescription },
                    { PlastekServiceError.MaxAmountError, PlastekResources.MaxAmountErrorDescription },
                    { PlastekServiceError.DataNotFound, PlastekResources.DataNotFoundDescription },
                    { PlastekServiceError.InvalidLocation, PlastekResources.InvalidLocationDescription },
                    { PlastekServiceError.InvalidTerminal, PlastekResources.InvalidTerminalDescription },
                    { PlastekServiceError.InvalidUser, PlastekResources.InvalidUserDescription },
                    { PlastekServiceError.InvalidPassword, PlastekResources.InvalidPasswordDescription },
                    { PlastekServiceError.InvalidGroup, PlastekResources.InvalidGroupDescription },
                    { PlastekServiceError.InvalidOperation, PlastekResources.InvalidOperationDescription },
                    { PlastekServiceError.NonReloadableCard, PlastekResources.NonReloadableCardDescription },
                    { PlastekServiceError.InvalidProgram, PlastekResources.InvalidProgramDescription },
                    { PlastekServiceError.TooMuchData, PlastekResources.TooMuchDataDescription },
                    { PlastekServiceError.InvalidParam, PlastekResources.InvalidParamDescription },
                    { PlastekServiceError.MinAmountError, PlastekResources.MinAmountErrorDescription },
                    { PlastekServiceError.CustomerLoginExists, PlastekResources.CustomerLoginExistsDescription },
                    { PlastekServiceError.CustomerRegistered, PlastekResources.CustomerRegisteredDescription },
                    { PlastekServiceError.CustomerPassportRegistered, PlastekResources.CustomerPassportRegisteredDescription },
                    { PlastekServiceError.CardNotRegistered, PlastekResources.CardNotRegisteredDescription },
                    { PlastekServiceError.CardRegistered, PlastekResources.CardRegisteredDescription },
                    { PlastekServiceError.CardRegisteredAnotherLogin, PlastekResources.CardRegisteredAnotherLoginDescription },
                    { PlastekServiceError.CardAssigned, PlastekResources.CardAssignedDescription },
                    { PlastekServiceError.OperationDelayed, PlastekResources.OperationDelayedDescription },
                    { PlastekServiceError.AmountStepError, PlastekResources.AmountStepErrorDescription },
                    { PlastekServiceError.InvalidTraceNumber, PlastekResources.InvalidTraceNumberDescription },
                    { PlastekServiceError.InvalidShift, PlastekResources.InvalidShiftDescription },
                    { PlastekServiceError.InvalidDistributor, PlastekResources.InvalidDistributorDescription },
                    { PlastekServiceError.AuthorizedCard, PlastekResources.AuthorizedCardDescription },
                    { PlastekServiceError.NotAuthorizedCard, PlastekResources.NotAuthorizedCardDescription },
                    { PlastekServiceError.InvalidAuthorization, PlastekResources.InvalidAuthorizationDescription },
                    { PlastekServiceError.InvalidAuthorizationAmount, PlastekResources.InvalidAuthorizationAmountDescription },
                    { PlastekServiceError.RedeemLimitError, PlastekResources.RedeemLimitErrorDescription },
                    { PlastekServiceError.AuthDelayError, PlastekResources.AuthDelayErrorDescription },
                    { PlastekServiceError.Generic, PlastekResources.GenericDescription },
                    { PlastekServiceError.SvcUnhandled, PlastekResources.SvcUnhandledDescription },
                    { PlastekServiceError.Unknown, PlastekResources.UnknownDescription },
                    { PlastekServiceError.ConnectionError, PlastekResources.ConnectionErrorDescription }
                };
        }

        private const int giftCardType = 1;

        public static PlastekCustomerCardInfo GetCardInfo(PlastekServiceIdentity identity, string cardNumber)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));
            if (cardNumber == null)
                throw new ArgumentNullException(nameof(cardNumber));

            var plastekClient = new GiftCardServiceSoapClient();

            var expectedUriScheme = plastekClient.Endpoint.Binding.Scheme;
            if (string.Compare(expectedUriScheme, identity.ServiceUri.Scheme, StringComparison.OrdinalIgnoreCase) != 0)
                throw new PlastekServiceException(PlastekServiceError.InvalidUriProtocol,
                    string.Format(PlastekResources.IncorrectServiceUriSchemeFormat, expectedUriScheme, identity.ServiceUri.Scheme), null);

            plastekClient.Endpoint.Address = new EndpointAddress(identity.ServiceUri);
            var getCardInfoRequest = new GetCardInfoRequest(new TranObject
                {
                    //Idenity fields
                    MerchantID = identity.MerchantId,
                    LocationID = identity.LocationId,
                    TerminalID = identity.TerminalId,
                    Login = identity.OperatorLogin,
                    Password = identity.OperatorPassword,

                    //Customer card fields
                    CardNumber = cardNumber,
                    CardType = giftCardType
                });

            try
            {
                var result = plastekClient.GetCardInfo(getCardInfoRequest);
                plastekClient.Close();
                return new PlastekCustomerCardInfo(result.obj.Balance, (PlastekCustomerCardStatus)result.obj.Status);
            }
            catch (FaultException exception)
            {
                try
                {
                    var plastekErrorCode = (PlastekServiceError)Enum.Parse(typeof(PlastekServiceError), exception.Message);

                    plastekClient.Abort();
                    throw new PlastekServiceException(plastekErrorCode, ErrorCodesToDescriptions[plastekErrorCode], exception);

                }
                catch (ArgumentException e)
                {
                    AbortClientAndThrowWrappedException(plastekClient, PlastekServiceError.Unknown, e);
                }
                catch (OverflowException e)
                {
                    AbortClientAndThrowWrappedException(plastekClient, PlastekServiceError.Unknown, e);
                }
            }
            catch (CommunicationException e)
            {
                AbortClientAndThrowWrappedException(plastekClient, PlastekServiceError.ConnectionError, e);
            }
            catch (TimeoutException e)
            {
                AbortClientAndThrowWrappedException(plastekClient, PlastekServiceError.ConnectionError, e);
            }

            //Компилятор не знает, что AbortClientAndThrowWrappedException всегда кидает exception,
            //а значит, до этой строки мы не дойдем
            throw new InvalidOperationException();
        }

        private static void AbortClientAndThrowWrappedException([NotNull] ICommunicationObject plastekClient, PlastekServiceError plastekErrorCode, [NotNull] Exception innerException)
        {
            if (plastekClient == null)
                throw new ArgumentNullException(nameof(plastekClient));
            if (innerException == null)
                throw new ArgumentNullException(nameof(innerException));

            plastekClient.Abort();
            throw new PlastekServiceException(plastekErrorCode, ErrorCodesToDescriptions[plastekErrorCode], innerException);
        }
    }
}
