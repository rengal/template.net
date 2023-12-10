using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.Plastek
{
    /// <summary>
    /// Данные для подключения к сервису Пластек
    /// </summary>
    public sealed class PlastekServiceIdentity
    {
        private readonly int merchantId;
        private readonly int locationId;
        private readonly string terminalId;
        private readonly string operatorLogin;
        private readonly string operatorPassword;
        private readonly Uri serviceUri;

        /// <summary>
        /// Id сети
        /// </summary>
        public int MerchantId
        {
            get { return merchantId; }
        }

        /// <summary>
        /// Id заведения
        /// </summary>
        public int LocationId
        {
            get { return locationId; }
        }

        /// <summary>
        /// Id терминала
        /// </summary>
        [NotNull]
        public string TerminalId
        {
            get { return terminalId; }
        }

        /// <summary>
        /// Логин оператора
        /// </summary>
        [NotNull]
        public string OperatorLogin
        {
            get { return operatorLogin; }
        }

        /// <summary>
        /// Пароль оператора
        /// </summary>
        [NotNull]
        public string OperatorPassword
        {
            get { return operatorPassword; }
        }

        /// <summary>
        /// Адрес сервиса Пластек
        /// </summary>
        [NotNull]
        public Uri ServiceUri
        {
            get { return serviceUri; }
        }

        /// <param name="merchantId">Id сети</param>
        /// <param name="locationId">Id заведения</param>
        /// <param name="terminalId">Id терминала</param>
        /// <param name="operatorLogin">Логин оператора</param>
        /// <param name="operatorPassword">Пароль оператора</param>
        /// <param name="serviceUri">Адрес сервиса Пластек</param>
        public PlastekServiceIdentity(int merchantId, int locationId, [NotNull] string terminalId, [NotNull] string operatorLogin,
                                      [NotNull] string operatorPassword, [NotNull] Uri serviceUri)
        {
            if (terminalId == null)
                throw new ArgumentNullException(nameof(terminalId));
            if (operatorLogin == null)
                throw new ArgumentNullException(nameof(operatorLogin));
            if (operatorPassword == null)
                throw new ArgumentNullException(nameof(operatorPassword));
            if (serviceUri == null)
                throw new ArgumentNullException(nameof(serviceUri));
            if (!serviceUri.IsAbsoluteUri)
                throw new ArgumentException("serviceUri should be absolute");

            this.merchantId = merchantId;
            this.terminalId = terminalId;
            this.locationId = locationId;
            this.operatorLogin = operatorLogin;
            this.operatorPassword = operatorPassword;
            this.serviceUri = serviceUri;
        }
    }
}