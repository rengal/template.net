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
    /// Элемент заказа
    /// </summary>
    public interface IKitchenOrderItem
    {
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Продукт/блюдо
        /// </summary>
        [NotNull]
        IProduct Product { get; }

        /// <summary>
        /// Статус обработки блюда
        /// </summary>
        [Obsolete("Оставлено для обратной совместимости, т.к. кухонные статусы приготовления блюд были изменены и расширены.")]
        KitchenProcessingStatus ProcessingStatus { get; }

        /// <summary>
        /// Статус обработки блюда
        /// </summary>
        KitchenProcessingStatusExtended ProcessingStatusExtended { get; }

        /// <summary>
        /// Кухня, в которой готовится блюдо
        /// </summary>
        [NotNull]
        IRestaurantSection Kitchen { get; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        bool Deleted { get; }

        /// <summary>
        /// Время удаления
        /// </summary>
        DateTime? DeleteTime { get; }

        /// <summary>
        /// Время начала приготовления
        /// </summary>
        [Obsolete("Оставлено для обратной совместимости, т.к. кухонные статусы приготовления блюд были изменены и расширены.")]
        DateTime? ProcessingBeginTime { get; }

        /// <summary>
        /// Время начала первого этапа приготовления
        /// </summary>
        DateTime? Processing1BeginTime { get; }

        /// <summary>
        /// Время начала второго этапа приготовления
        /// </summary>
        DateTime? Processing2BeginTime { get; }

        /// <summary>
        /// Время начала третьего этопа приготовления
        /// </summary>
        DateTime? Processing3BeginTime { get; }

        /// <summary>
        /// Время начала четвертого этапа приготовления
        /// </summary>
        DateTime? Processing4BeginTime { get; }

        /// <summary>
        /// Время окончания приготовления
        /// </summary>
        DateTime? ProcessingCompletedTime { get; }

        /// <summary>
        /// Время подачи
        /// </summary>
        DateTime? ServeTime { get; }

    }

    internal abstract class KitchenOrderItem : TemplateModelBase, IKitchenOrderItem
    {
        #region Fields
        private readonly decimal amount;
        private readonly Product product;
        private readonly KitchenProcessingStatus processingStatus;
        private readonly KitchenProcessingStatusExtended processingStatusExtended;
        private readonly RestaurantSection kitchen;
        private readonly bool deleted;
        private readonly DateTime? deleteTime;
        private readonly DateTime? processingBeginTime;
        private readonly DateTime? processing1BeginTime;
        private readonly DateTime? processing2BeginTime;
        private readonly DateTime? processing3BeginTime;
        private readonly DateTime? processing4BeginTime;
        private readonly DateTime? processingCompletedTime;
        private readonly DateTime? serveTime;
        #endregion

        #region Ctor
        protected KitchenOrderItem()
        {}

        protected KitchenOrderItem([NotNull] CopyContext context, [NotNull] IKitchenOrderItem src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            amount = src.Amount;
            product = context.GetConverted(src.Product, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Product.Convert);
            processingStatus = src.ProcessingStatus;
            processingStatusExtended = src.ProcessingStatusExtended;
            kitchen = context.GetConverted(src.Kitchen, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.RestaurantSection.Convert);
            deleted = src.Deleted;
            deleteTime = src.DeleteTime;
            processingBeginTime = src.ProcessingBeginTime;
            processing1BeginTime = src.Processing1BeginTime;
            processing2BeginTime = src.Processing2BeginTime;
            processing3BeginTime = src.Processing3BeginTime;
            processing4BeginTime = src.Processing4BeginTime;
            processingCompletedTime = src.ProcessingCompletedTime;
            serveTime = src.ServeTime;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static KitchenOrderItem Convert([NotNull] CopyContext context, [CanBeNull] IKitchenOrderItem source)
        {
            if (source == null)
                return null;

            if (source is IKitchenOrderModifierItem)
                return KitchenOrderModifierItem.Convert(context, (IKitchenOrderModifierItem)source);
            else if (source is IKitchenOrderProductItem)
                return KitchenOrderProductItem.Convert(context, (IKitchenOrderProductItem)source);
            else
                throw new ArgumentException(string.Format("Type {0} not supported", source.GetType()), "source");
        }
        #endregion

        #region Props
        public decimal Amount
        {
            get { return amount; }
        }

        public IProduct Product
        {
            get { return product; }
        }

        public KitchenProcessingStatus ProcessingStatus
        {
            get { return processingStatus; }
        }

        public KitchenProcessingStatusExtended ProcessingStatusExtended
        {
            get { return processingStatusExtended; }
        }

        public IRestaurantSection Kitchen
        {
            get { return kitchen; }
        }

        public bool Deleted
        {
            get { return deleted; }
        }

        public DateTime? DeleteTime
        {
            get { return deleteTime; }
        }

        public DateTime? ProcessingBeginTime
        {
            get { return processingBeginTime; }
        }

        public DateTime? Processing1BeginTime
        {
            get { return processing1BeginTime; }
        }

        public DateTime? Processing2BeginTime
        {
            get { return processing2BeginTime; }
        }

        public DateTime? Processing3BeginTime
        {
            get { return processing3BeginTime; }
        }

        public DateTime? Processing4BeginTime
        {
            get { return processing4BeginTime; }
        }

        public DateTime? ProcessingCompletedTime
        {
            get { return processingCompletedTime; }
        }

        public DateTime? ServeTime
        {
            get { return serveTime; }
        }

        #endregion
    }

}
