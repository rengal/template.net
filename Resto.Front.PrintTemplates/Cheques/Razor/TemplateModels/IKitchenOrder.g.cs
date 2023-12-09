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
    /// Кухонный заказ
    /// </summary>
    public interface IKitchenOrder
    {
        /// <summary>
        /// Номер
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Номер данного заказа во внешней системе (на сайте агрегатора и т.п.)
        /// </summary>
        [CanBeNull]
        string ExternalNumber { get; }

        /// <summary>
        /// Флаг доставочного заказа
        /// </summary>
        bool IsDeliveryOrder { get; }

        /// <summary>
        /// Тип заказа
        /// </summary>
        [CanBeNull]
        IOrderType Type { get; }

        /// <summary>
        /// Официант
        /// </summary>
        [CanBeNull]
        IUser Waiter { get; }

        /// <summary>
        /// Стол
        /// </summary>
        [NotNull]
        ITable Table { get; }

        /// <summary>
        /// Дата/время открытия
        /// </summary>
        DateTime OpenTime { get; }

        /// <summary>
        /// Номинальное количество гостей
        /// </summary>
        int InitialGuestsCount { get; }

        /// <summary>
        /// Количество гостей
        /// </summary>
        int ActualGuestsCount { get; }

        /// <summary>
        /// Базовый заказ
        /// </summary>
        [CanBeNull]
        IOrder BaseOrder { get; }

        /// <summary>
        /// Элементы заказа
        /// </summary>
        [NotNull]
        IEnumerable<IKitchenOrderProductItem> Items { get; }

        /// <summary>
        /// Общедоступные внешние данные, хранимые API-плагинами в заказе.
        /// </summary>
        [NotNull]
        IDictionary<string, string> ExternalData { get; }

    }

    internal sealed class KitchenOrder : TemplateModelBase, IKitchenOrder
    {
        #region Fields
        private readonly int number;
        private readonly string externalNumber;
        private readonly bool isDeliveryOrder;
        private readonly OrderType type;
        private readonly User waiter;
        private readonly Table table;
        private readonly DateTime openTime;
        private readonly int initialGuestsCount;
        private readonly int actualGuestsCount;
        private readonly Order baseOrder;
        private readonly List<KitchenOrderProductItem> items = new List<KitchenOrderProductItem>();
        private readonly Dictionary<string, string> externalData = new Dictionary<string, string>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private KitchenOrder()
        {}

        private KitchenOrder([NotNull] CopyContext context, [NotNull] IKitchenOrder src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            number = src.Number;
            externalNumber = src.ExternalNumber;
            isDeliveryOrder = src.IsDeliveryOrder;
            type = context.GetConverted(src.Type, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.OrderType.Convert);
            waiter = context.GetConverted(src.Waiter, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            table = context.GetConverted(src.Table, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Table.Convert);
            openTime = src.OpenTime;
            initialGuestsCount = src.InitialGuestsCount;
            actualGuestsCount = src.ActualGuestsCount;
            baseOrder = context.GetConverted(src.BaseOrder, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            items = src.Items.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderProductItem.Convert)).ToList();
            externalData = new Dictionary<string, string>(src.ExternalData);
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static KitchenOrder Convert([NotNull] CopyContext context, [CanBeNull] IKitchenOrder source)
        {
            if (source == null)
                return null;

            return new KitchenOrder(context, source);
        }
        #endregion

        #region Props
        public int Number
        {
            get { return number; }
        }

        public string ExternalNumber
        {
            get { return GetLocalizedValue(externalNumber); }
        }

        public bool IsDeliveryOrder
        {
            get { return isDeliveryOrder; }
        }

        public IOrderType Type
        {
            get { return type; }
        }

        public IUser Waiter
        {
            get { return waiter; }
        }

        public ITable Table
        {
            get { return table; }
        }

        public DateTime OpenTime
        {
            get { return openTime; }
        }

        public int InitialGuestsCount
        {
            get { return initialGuestsCount; }
        }

        public int ActualGuestsCount
        {
            get { return actualGuestsCount; }
        }

        public IOrder BaseOrder
        {
            get { return baseOrder; }
        }

        public IEnumerable<IKitchenOrderProductItem> Items
        {
            get { return items; }
        }

        public IDictionary<string, string> ExternalData
        {
            get { return externalData; }
        }

        #endregion
    }

}
