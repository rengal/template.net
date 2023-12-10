using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Data;

namespace Resto.Common.WatchDog
{
    /// <summary>
    /// Класс, отвечающий за детектирование ситуации, когда приближается срок окончания действия лицензии.
    /// </summary>
    public class LicenseExpirationObserver
    {
        #region Fields

        /// <summary>
        /// Синглтон (обновляется после каждого успешного входа в систему).
        /// </summary>
        public static LicenseExpirationObserver Instance;

        /// <summary>
        /// Запасной интервал времени. Когда до момента истечения срока действия лицензии остается времени меньше, 
        /// чем данный интервал, выводится сообщение о том, что все вкладки и окна будут закрыты.
        /// Вкладки закрываются с возможностью сохранения данных (т.к. до истечения лицензии еще есть немного времени).
        /// </summary>
        public static readonly TimeSpan ExpirationReservePeriod = new TimeSpan(0, 10, 0);

        private readonly IWatchDogCheckResultsManager watchDogCheckResultsManager;

        #endregion

        #region Events

        /// <summary>
        /// Событие детектирования скорого истечения лицензии.
        /// </summary>
        public event EventHandler<LicenseExpiredEventArgs> LicenseExpired;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="watchDogCheckResultsManager">Менеджер результатов проверок WatchDog.</param>
        public LicenseExpirationObserver(IWatchDogCheckResultsManager watchDogCheckResultsManager)
        {
            IsExpired = false;
            this.watchDogCheckResultsManager = watchDogCheckResultsManager;
            watchDogCheckResultsManager.CheckResultsReceived += watchDogCheckResultsManager_CheckResultsReceived;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Признак того, что лицензия истекла либо истекает менее чем через 10 минут.
        /// </summary>
        public bool IsExpired { get; private set; }

        /// <summary>
        /// <c>true</c>, если лицензия уже истекла либо до момента ее истечения осталось менее 10 минут.
        /// </summary>
        public static bool IsLicenseExpired
        {
            get { return Instance != null && Instance.IsExpired; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обрабатывает актуальные на настоящий момент результаты проверок WatchDog
        /// на предмет детектирования истечения срока действия лицензии.
        /// </summary>
        /// <param name="onLogin">Признак проведения обработки непосредственно после успешного входа в систему.</param>
        public void ProcessCheckResults(bool onLogin = false)
        {
            if (!IsExpired
                && watchDogCheckResultsManager.ActualCheckResults
                                              .OfType<LicenseCheckResult>()
                                              .Any(r =>
                                                   r.ExpireSeconds != null &&
                                                   r.ExpireSeconds < ExpirationReservePeriod.TotalSeconds))
            {
                IsExpired = true;

                LicenseExpired(this, new LicenseExpiredEventArgs(onLogin));
            }
        }

        #endregion

        #region Event handlers

        private void watchDogCheckResultsManager_CheckResultsReceived(object sender, EventArgs e)
        {
            ProcessCheckResults();
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Аргументы события истечения срока действия лицензии.
        /// </summary>
        public class LicenseExpiredEventArgs : EventArgs
        {
            public LicenseExpiredEventArgs(bool observedOnLogin)
            {
                DetectedOnLogon = observedOnLogin;
            }

            public bool DetectedOnLogon { get; set; }
        }

        #endregion
    }
}
