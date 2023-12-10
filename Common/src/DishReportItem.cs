using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

namespace Resto.Data
{
    /// <summary>
    /// Информация о блюде для отчёта "Банкеты, резервы, доставки"
    /// </summary>
    public class DishReportItem : ItemSaleEvent
    {
        private DishReportItem([NotNull] IItemWithDishInfo itemWithDishInfo)
        {
            Id = itemWithDishInfo.Id;
            DishInfo = itemWithDishInfo.DishInfo;
            Store = itemWithDishInfo.Store;
            DishAmount = itemWithDishInfo.DishAmount.GetValueOrFakeDefault();
            DishPrice = itemWithDishInfo.DishPrice.GetValueOrFakeDefault();
            DishSum = itemWithDishInfo.DishSum.GetValueOrFakeDefault();

            IsBanquetSaleItem = itemWithDishInfo is BanquetSaleItem;
            IsDeliverySaleItem = itemWithDishInfo is DeliverySaleItem;

            var itemSaleEvent = itemWithDishInfo as ItemSaleEvent;
            if (itemSaleEvent != null)
            {
                OrderId = itemSaleEvent.OrderId;
                IsItemSaleEvent = true;
            }
        }

        public DishReportItem([NotNull] IItemWithDishInfo itemSaleEvent, Guid? parentId, DateTime dateTimeToPrepare, DateTime? dateTimeToDeliver)
            : this(itemSaleEvent)
        {
            Parent = parentId;
            DateTimeToPrepare = dateTimeToPrepare;
            DateTimeToDeliver = dateTimeToDeliver;
        }

        /// <summary>
        /// Время приготовления блюда
        /// </summary>
        public DateTime DateTimeToPrepare { get; private set; }

        /// <summary>
        /// Время доставки блюда (для банкетов/резервов - пусто)
        /// </summary>
        public DateTime? DateTimeToDeliver { get; private set; }

        /// <summary>
        /// Идентификатор родительского блюда (для модификаторов)
        /// </summary>
        public Guid? Parent { get; private set; }

        /// <summary>
        /// Строковое представление блюда, включает id модификаторов и их количество (для группировки в отчёте по блюдам)
        /// </summary>
        public string StringRepresentation { get; set; }

        /// <summary>
        /// Признак того, что данная запись - из открытого банкетного заказа (BanquetSaleItem)
        /// </summary>
        public bool IsBanquetSaleItem { get; private set; }

        /// <summary>
        /// Признак того, что данная запись - из открытой доставки (DeliverySaleItem)
        /// </summary>
        public bool IsDeliverySaleItem { get; private set; }

        /// <summary>
        /// Признак того, что данная запись - из закрытого заказа (ItemSaleEvent)
        /// </summary>
        public bool IsItemSaleEvent { get; private set; }

        private readonly ICollection<DishReportItem> modifiers = new List<DishReportItem>();
        /// <summary>
        /// Коллекция модификаторов (для основных блюд)
        /// </summary>
        public ICollection<DishReportItem> Modifiers
        {
            get { return modifiers; }
        }
    }
}
