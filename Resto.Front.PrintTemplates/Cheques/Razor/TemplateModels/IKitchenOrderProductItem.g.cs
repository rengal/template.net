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
    /// Элемент заказа (блюдо)
    /// </summary>
    public interface IKitchenOrderProductItem : IKitchenOrderItem
    {
        /// <summary>
        /// Номер курса
        /// </summary>
        int Course { get; }

        /// <summary>
        /// Время печати блюда на кухню
        /// </summary>
        DateTime PrintTime { get; }

        /// <summary>
        /// Номер группы подачи
        /// </summary>
        int ServeGroupNumber { get; }

        /// <summary>
        /// Величина для сортировки блюд в заказе
        /// </summary>
        int OrderRank { get; }

        /// <summary>
        /// Имя гостя, заказавшего блюдо
        /// </summary>
        string GuestName { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        IKitchenOrderProductItemComment Comment { get; }

        /// <summary>
        /// Размер
        /// </summary>
        [CanBeNull]
        IProductItemSize ProductSize { get; }

        /// <summary>
        /// Штрихкод
        /// </summary>
        [CanBeNull]
        string Barcode { get; }

        /// <summary>
        /// Штрихкод для сервисного чека
        /// </summary>
        [NotNull]
        string ServiceChequeBarcode { get; }

        /// <summary>
        /// Информация о компонентах составных блюд
        /// </summary>
        [CanBeNull]
        ICompoundOrderItemInfo CompoundsInfo { get; }

        /// <summary>
        /// Модификаторы
        /// </summary>
        [NotNull]
        IEnumerable<IKitchenOrderModifierItem> Modifiers { get; }

        /// <summary>
        /// Продукты для модификаторов с нулевым количеством
        /// </summary>
        [NotNull]
        IEnumerable<IProduct> ProductsOfZeroAmountModifiers { get; }

        /// <summary>
        /// Продукты для общих модификаторов с нулевым количеством
        /// </summary>
        [NotNull]
        IEnumerable<IProduct> ProductsOfZeroAmountCommonModifiers { get; }

    }

    internal sealed class KitchenOrderProductItem : KitchenOrderItem, IKitchenOrderProductItem
    {
        #region Fields
        private readonly int course;
        private readonly DateTime printTime;
        private readonly int serveGroupNumber;
        private readonly int orderRank;
        private readonly string guestName;
        private readonly KitchenOrderProductItemComment comment;
        private readonly ProductItemSize productSize;
        private readonly string barcode;
        private readonly string serviceChequeBarcode;
        private readonly CompoundOrderItemInfo compoundsInfo;
        private readonly List<KitchenOrderModifierItem> modifiers = new List<KitchenOrderModifierItem>();
        private readonly List<Product> productsOfZeroAmountModifiers = new List<Product>();
        private readonly List<Product> productsOfZeroAmountCommonModifiers = new List<Product>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private KitchenOrderProductItem()
        {}

        private KitchenOrderProductItem([NotNull] CopyContext context, [NotNull] IKitchenOrderProductItem src)
            : base(context, src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            course = src.Course;
            printTime = src.PrintTime;
            serveGroupNumber = src.ServeGroupNumber;
            orderRank = src.OrderRank;
            guestName = src.GuestName;
            comment = context.GetConverted(src.Comment, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderProductItemComment.Convert);
            productSize = context.GetConverted(src.ProductSize, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ProductItemSize.Convert);
            barcode = src.Barcode;
            serviceChequeBarcode = src.ServiceChequeBarcode;
            compoundsInfo = context.GetConverted(src.CompoundsInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CompoundOrderItemInfo.Convert);
            modifiers = src.Modifiers.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.KitchenOrderModifierItem.Convert)).ToList();
            productsOfZeroAmountModifiers = src.ProductsOfZeroAmountModifiers.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Product.Convert)).ToList();
            productsOfZeroAmountCommonModifiers = src.ProductsOfZeroAmountCommonModifiers.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Product.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static KitchenOrderProductItem Convert([NotNull] CopyContext context, [CanBeNull] IKitchenOrderProductItem source)
        {
            if (source == null)
                return null;

            return new KitchenOrderProductItem(context, source);
        }
        #endregion

        #region Props
        public int Course
        {
            get { return course; }
        }

        public DateTime PrintTime
        {
            get { return printTime; }
        }

        public int ServeGroupNumber
        {
            get { return serveGroupNumber; }
        }

        public int OrderRank
        {
            get { return orderRank; }
        }

        public string GuestName
        {
            get { return GetLocalizedValue(guestName); }
        }

        public IKitchenOrderProductItemComment Comment
        {
            get { return comment; }
        }

        public IProductItemSize ProductSize
        {
            get { return productSize; }
        }

        public string Barcode
        {
            get { return GetLocalizedValue(barcode); }
        }

        public string ServiceChequeBarcode
        {
            get { return GetLocalizedValue(serviceChequeBarcode); }
        }

        public ICompoundOrderItemInfo CompoundsInfo
        {
            get { return compoundsInfo; }
        }

        public IEnumerable<IKitchenOrderModifierItem> Modifiers
        {
            get { return modifiers; }
        }

        public IEnumerable<IProduct> ProductsOfZeroAmountModifiers
        {
            get { return productsOfZeroAmountModifiers; }
        }

        public IEnumerable<IProduct> ProductsOfZeroAmountCommonModifiers
        {
            get { return productsOfZeroAmountCommonModifiers; }
        }

        #endregion
    }

}
