// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
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
        /// Столы
        /// </summary>
        [NotNull]
        IEnumerable<ITable> Tables { get; }

    }

    internal sealed class Reserve : TemplateModelBase, IReserve
    {
        #region Fields
        private readonly bool isBanquet;
        private readonly int number;
        private readonly DateTime startTime;
        private readonly TimeSpan duration;
        private readonly int guestsCount;
        private readonly string activityType;
        private readonly string comment;
        private readonly Customer customer;
        private readonly string phone;
        private readonly Order order;
        private readonly DateTime lastModifiedTime;
        private readonly List<Table> tables = new List<Table>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Reserve()
        {}

        private Reserve([NotNull] CopyContext context, [NotNull] IReserve src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            isBanquet = src.IsBanquet;
            number = src.Number;
            startTime = src.StartTime;
            duration = src.Duration;
            guestsCount = src.GuestsCount;
            activityType = src.ActivityType;
            comment = src.Comment;
            customer = context.GetConverted(src.Customer, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Customer.Convert);
            phone = src.Phone;
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            lastModifiedTime = src.LastModifiedTime;
            tables = src.Tables.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Table.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Reserve Convert([NotNull] CopyContext context, [CanBeNull] IReserve source)
        {
            if (source == null)
                return null;

            return new Reserve(context, source);
        }
        #endregion

        #region Props
        public bool IsBanquet
        {
            get { return isBanquet; }
        }

        public int Number
        {
            get { return number; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
        }

        public TimeSpan Duration
        {
            get { return duration; }
        }

        public int GuestsCount
        {
            get { return guestsCount; }
        }

        public string ActivityType
        {
            get { return GetLocalizedValue(activityType); }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        public ICustomer Customer
        {
            get { return customer; }
        }

        public string Phone
        {
            get { return GetLocalizedValue(phone); }
        }

        public IOrder Order
        {
            get { return order; }
        }

        public DateTime LastModifiedTime
        {
            get { return lastModifiedTime; }
        }

        public IEnumerable<ITable> Tables
        {
            get { return tables; }
        }

        #endregion
    }

}
