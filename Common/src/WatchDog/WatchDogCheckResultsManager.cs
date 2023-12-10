using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Resto.Data;
using Resto.Framework;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.WatchDog
{
    /// <summary>
    /// Класс, отвечающий за прием, хранение и учет просмотра пользователем результатов проверок WatchDog
    /// </summary>
    public class WatchDogCheckResultsManager : IWatchDogCheckResultsManager
    {
        #region Fields
        /// <summary>
        /// Наиболее актуальные результаты проверок WatchDog
        /// </summary>
        private readonly List<CheckResult> actualCheckResults = new List<CheckResult>();
        /// <summary>
        /// Результаты проверок WatchDog, которые были отображены в последний раз либо в окне общей системно1й диагностики, 
        /// либо во всплывабщем окне ошибок и предупреждений WatchDog
        /// </summary>
        private readonly List<CheckResult> lastShownCheckResults = new List<CheckResult>();
        /// <summary>
        /// Объект для синхронизации
        /// </summary>
        private readonly object locker = new object();
        /// <summary>
        /// Компарер для <see cref="CheckResult"/>
        /// </summary>
        private readonly CheckResultEqualityComparer checkResultComparer = new CheckResultEqualityComparer();

        #endregion

        #region Events

        /// <summary>
        /// Событие обновления состояния флага <see cref="HasNotShownCheckResults"/> после прихода новых
        /// уведомлений WatchDog или просмотра пользователем ранее полученных уведомлений
        /// </summary>
        public event EventHandler HasNotShownCheckResultsUpdated;

        /// <summary>
        /// Событие получения на обработку коллекции результатов проверок WatchDog.
        /// </summary>
        public event EventHandler CheckResultsReceived;

        #endregion

        #region Properties

        [NotNull]
        public static IWatchDogCheckResultsManager Instance { get; private set; }

        /// <summary>
        /// Самые свежие результаты проверок WatchDog.
        /// (копия, коллекция только для чтения)
        /// </summary>
        public ICollection<CheckResult> ActualCheckResults
        {
            get
            {
                lock (locker)
                {
                    return new ReadOnlyCollection<CheckResult>(actualCheckResults.ToArray());
                }
            }
        }

        /// <summary>
        /// Признак наличия результатов проверок, которые пользователь еще не просматривал
        /// </summary>
        public bool HasNotShownCheckResults { get; private set; }

        public DateTime LastTimeWatchDogCheckResultsShown { get; set; }

        #endregion

        #region Methods

        public static void Init()
        {
            Instance = new WatchDogCheckResultsManager();
        }

        /// <summary>
        /// Предназначен для передачи "менеджеру" новой порции результатов проверок WatchDog
        /// </summary>
        public void OnCheckResultsReceived(ICollection<CheckResult> checkResults)
        {
            Contract.Requires(checkResults != null);

            lock (locker)
            {
                bool resultsEqual = actualCheckResults.SequenceEqual(checkResults, checkResultComparer);

                actualCheckResults.Clear();
                actualCheckResults.AddRange(checkResults);

                if (!resultsEqual)
                {
                    // Убираем из показанных все неактуальные результаты проверок WatchDog.
                    // Пример случая, зачем это нужно:
                    // Пусть сообщение "А" было показано в 10:00. 
                    // В 12:00 оно исчезло из актуальных результатов проверок (проблема ушла).
                    // А в 14:00 оно вновь попало в результаты проверок (проблема вернулась :)).
                    // В этом случае данное сообщение должно быть показано повторно, 
                    // т.к. по сути это уже новая ошибка.
                    lastShownCheckResults.RemoveAll(
                        shownCheckResult => !actualCheckResults.Any(
                            actualCheckResult => checkResultComparer.Equals(actualCheckResult, shownCheckResult)));

                    UpdateHasNotShownCheckResults();
                }
            }

            if (CheckResultsReceived != null)
            {
                CheckResultsReceived(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// <para>Обновляет <see cref="lastShownCheckResults"/> и <see cref="HasNotShownCheckResults"/>.</para>
        /// <para>Необходимо вызывать после того, как пользователь просмотрел текущие сообщения об ошибках WatchDog.</para>
        /// </summary>
        /// <param name="checkResults">Сообщения WatchDog, которые просмотрел пользователь.</param>
        public void UpdateShownCheckResults(ICollection<CheckResult> checkResults)
        {
            Contract.Requires(checkResults != null);

            lock (locker)
            {
                if (lastShownCheckResults.SequenceEqual(checkResults, checkResultComparer))
                {
                    return;
                }

                lastShownCheckResults.Clear();
                lastShownCheckResults.AddRange(checkResults);

                UpdateHasNotShownCheckResults();
            }
        }

        /// <summary>
        /// Обновляет одновременно и текущие актуальные результаты проверок WatchDog, и последние отображенные результаты
        /// </summary>
        /// <param name="checkResults">Сообщения WatchDog</param>
        public void UpdateActualAndShownCheckResults(ICollection<CheckResult> checkResults)
        {
            Contract.Requires(checkResults != null);

            lock (locker)
            {
                actualCheckResults.Clear();
                lastShownCheckResults.Clear();

                actualCheckResults.AddRange(checkResults);
                lastShownCheckResults.AddRange(checkResults);

                UpdateHasNotShownCheckResults();
            }
        }

        /// <summary>
        /// <para>Обновляет состояние <see cref="HasNotShownCheckResults"/>.</para>
        /// <para>Должен вызываться в контексте блокировки <see cref="locker"/>.</para>
        /// </summary>
        private void UpdateHasNotShownCheckResults()
        {
            HasNotShownCheckResults =
                actualCheckResults.Any(
                    actualCheckResult =>
                    !lastShownCheckResults.Any(shownCheckResult => checkResultComparer.Equals(actualCheckResult, shownCheckResult)));

            //GuiHelper.InvokeLater(OnHasNotShownCheckResultsUpdated); //todo debugnow
        }

        /// <summary>
        /// Генерирует событие <see cref="HasNotShownCheckResultsUpdated"/>
        /// </summary>
        private void OnHasNotShownCheckResultsUpdated()
        {
            if (HasNotShownCheckResultsUpdated != null)
            {
                HasNotShownCheckResultsUpdated(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Сравнивает объекты <see cref="CheckResult"/>, исходя из их содержимого
        /// </summary>
        private class CheckResultEqualityComparer : IEqualityComparer<CheckResult>
        {
            public bool Equals(CheckResult x, CheckResult y)
            {
                bool result = ReferenceEquals(x, y);

                if (!result && x != null && y != null)
                {
                    result = x.CheckerId == y.CheckerId && x.Severity == y.Severity && x.Message == y.Message;
                }

                return result;
            }

            public int GetHashCode(CheckResult obj)
            {
                return (obj.CheckerId + "~" + obj.Message + "~" + obj.Severity._Value).GetHashCode();
            }
        }

        #endregion
    }
}
