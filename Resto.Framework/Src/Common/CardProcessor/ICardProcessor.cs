using System;

namespace Resto.Framework.Common.CardProcessor
{
    using CardRollFailedHandler = EventHandler<EventArgs<object>>;

    ///<summary>
    /// Интерфейс, описывающий непосредственный процессор распознавания карты
    ///</summary>
    public interface ICardProcessor
    {
        /// <summary>
        /// Подписаться на событие прокатки карты.
        /// </summary>
        void AddCardRolledHandler(EventHandler<CardRolledEventArgs> cardRolledHandler, string type);
        void AddCardRolledHandler(EventHandler<CardRolledEventArgs> cardRolledHandler, CardRollFailedHandler errorHandler, string type);
        void AddCardRolledHandler(EventHandler<CardRolledEventArgs> cardRolledHandler, CardRollFailedHandler failedHandler, string type, int priority);

        /// <summary>
        /// Отписаться от события прокатки карты.
        /// </summary>
        void RemoveCardRolledHandler(EventHandler<CardRolledEventArgs> cardRolledHandler);

        /// <summary>
        /// Подключить все доступные на данный момент процессоры.
        /// </summary>
        void Attach();

        /// <summary>
        /// Отключить все доступные на данный момент процессоры.
        /// </summary>
        void Detach();

        /// <summary>
        /// Устанавливает верхний попрог по приоритету для парсеров и процессоров.
        /// </summary>
        /// <remarks>
        /// Парсеры и процессоры приоритет которых выше <paramref name="priorityThreshold"/>, 
        /// не будут вызываться, пока не будет уничтожен объект, который возвращает функция.
        /// </remarks>
        /// <param name="priorityThreshold">Порог приоритета.</param>
        IDisposable CreateProcessorPriorityThresholdHolder(int priorityThreshold);
    }
}