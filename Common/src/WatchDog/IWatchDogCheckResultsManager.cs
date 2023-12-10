using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;

namespace Resto.Common.WatchDog
{
    /// <summary>
    /// Интерфейс "менеджера" получаемых на регулярной основе результатов проверок WatchDog
    /// </summary>
    public interface IWatchDogCheckResultsManager
    {
        /// <summary>
        /// Событие обновления состояния флага <see cref="HasNotShownCheckResults"/> после прихода новых
        /// уведомлений WatchDog или просмотра пользователем ранее полученных уведомлений
        /// </summary>
        event EventHandler HasNotShownCheckResultsUpdated;

        /// <summary>
        /// Событие получения результатов проверок WatchDog.
        /// </summary>
        event EventHandler CheckResultsReceived;

        /// <summary>
        /// Самые свежие результаты проверок WatchDog
        /// </summary>
        ICollection<CheckResult> ActualCheckResults { get; }

        /// <summary>
        /// Признак наличия результатов проверок, которые пользователь еще не просматривал
        /// </summary>
        bool HasNotShownCheckResults { get; }

        /// <summary>
        /// Последний момент времени, когда показывались сообщения WatchDog
        /// </summary>
        DateTime LastTimeWatchDogCheckResultsShown { get; set; }

        /// <summary>
        /// Предназначен для передачи "менеджеру" новой порции результатов проверок WatchDog
        /// </summary>
        void OnCheckResultsReceived([NotNull] ICollection<CheckResult> checkResults);

        /// <summary>
        /// <para>Обновляет <see cref="WatchDogCheckResultsManager.lastShownCheckResults"/> и <see cref="WatchDogCheckResultsManager.HasNotShownCheckResults"/>.</para>
        /// <para>Необходимо вызывать после того, как пользователь просмотрел текущие сообщения об ошибках WatchDog.</para>
        /// </summary>
        /// <param name="checkResults">Сообщения WatchDog, которые просмотрел пользователь.</param>
        void UpdateShownCheckResults([NotNull] ICollection<CheckResult> checkResults);

        /// <summary>
        /// Обновляет одновременно и текущие актуальные результаты проверок WatchDog, и последние отображенные результаты
        /// </summary>
        /// <param name="checkResults">Сообщения WatchDog</param>
        void UpdateActualAndShownCheckResults([NotNull] ICollection<CheckResult> checkResults);
    }
}