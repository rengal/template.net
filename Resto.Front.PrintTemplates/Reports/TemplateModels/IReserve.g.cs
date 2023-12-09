// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    /// <summary>
    /// Резерв/банкет
    /// </summary>
    public interface IReserve
    {
        /// <summary>
        /// Признак банкета
        /// </summary>
        bool IsBanquet { get; }

        /// <summary>
        /// Номер
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Дата/время начала
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Продолжительность
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Дата/время прихода гостей
        /// </summary>
        DateTime? GuestComingTime { get; }

        /// <summary>
        /// Количество гостей
        /// </summary>
        int GuestsCount { get; }

        /// <summary>
        /// Тип мероприятия
        /// </summary>
        [CanBeNull]
        string ActivityType { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Клиент
        /// </summary>
        [NotNull]
        ICustomer Customer { get; }

        /// <summary>
        /// Номер телефона клиента
        /// </summary>
        [CanBeNull]
        string Phone { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [CanBeNull]
        IOrder Order { get; }

        /// <summary>
        /// Время последнего изменения
        /// </summary>
        DateTime LastModifiedTime { get; }

        /// <summary>
        /// Статус
        /// </summary>
        ReserveStatus Status { get; }

        /// <summary>
        /// Причина отмены
        /// </summary>
        ReserveCancelCause CancelCause { get; }

        /// <summary>
        /// Столы
        /// </summary>
        [NotNull]
        IEnumerable<ITable> Tables { get; }

    }
}
